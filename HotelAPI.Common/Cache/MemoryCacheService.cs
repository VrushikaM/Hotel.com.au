using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.Common.Cache
{
	public class MemoryCacheService : ICacheService
	{
		private readonly IMemoryCache _cache;

		public MemoryCacheService(IMemoryCache cache)
		{
			_cache = cache;
		}

		public async Task<T> GetOrCreateAsync<T>(
			string cacheKey,
			Func<Task<T>> factory,
			TimeSpan expiration,
			TimeSpan? slidingExpiration = null)
		{
			return await _cache.GetOrCreateAsync(cacheKey, async entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = expiration;

				if (slidingExpiration.HasValue)
					entry.SlidingExpiration = slidingExpiration;

				return await factory();
			}) ?? default!;
		}

		public void Remove(string cacheKey)
		{
			_cache.Remove(cacheKey);
		}
	}
}
