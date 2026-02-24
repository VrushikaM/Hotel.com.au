namespace HotelAPI.Common.Cache
{
	public interface ICacheService
	{
		Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan expiration, TimeSpan? slidingExpiration = null);
		void Remove(string cacheKey);
	}
}
