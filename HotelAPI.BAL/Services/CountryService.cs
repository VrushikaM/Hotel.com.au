using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Country;
using Microsoft.Extensions.Caching.Memory;

namespace HotelAPI.BAL.Services
{
	public class CountryService : ICountryService
	{
		private readonly ICountryRepository _countryRepository;
		private readonly IMemoryCache _cache;

		private const string COUNTRY_LIST_CACHE_KEY = "COUNTRY_LIST";

		public CountryService(
			ICountryRepository countryRepository,
			IMemoryCache cache)
		{		
			_countryRepository = countryRepository;
			_cache = cache;
		}

		public async Task<ResponseResult<IEnumerable<CountryListResponse>>> GetCountryListAsync()
		{
			try
			{
				if (!_cache.TryGetValue(COUNTRY_LIST_CACHE_KEY, out IEnumerable<CountryListResponse>? data))
				{
					data = await _countryRepository.GetCountryListAsync();

					if (data == null || !data.Any())
					{
						return ResponseHelper<IEnumerable<CountryListResponse>>.Error(
							"No countries found",
							statusCode: StatusCode.NOT_FOUND
						);
					}

					var cacheOptions = new MemoryCacheEntryOptions
					{
						AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
					};

					_cache.Set(COUNTRY_LIST_CACHE_KEY, data, cacheOptions);
				}

				return ResponseHelper<IEnumerable<CountryListResponse>>.Success(
					"Country list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<CountryListResponse>>.Error(
					"Failed to fetch country list",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<CountryByUrlNameResponse>> GetCountryByUrlNameAsync(
			string urlName,
			string? alphabet)
		{
			try
			{
				var cacheKey = $"COUNTRY_URL_{urlName}_{alphabet}";

				if (!_cache.TryGetValue(cacheKey, out CountryByUrlNameResponse? data))
				{
					data = await _countryRepository.GetCountryByUrlNameAsync(urlName, alphabet);

					if (data == null)
					{
						return ResponseHelper<CountryByUrlNameResponse>.Error(
							"No country found",
							statusCode: StatusCode.NOT_FOUND
						);
					}

					_cache.Set(
						cacheKey,
						data,
						TimeSpan.FromMinutes(10)
					);
				}

				return ResponseHelper<CountryByUrlNameResponse>.Success(
					"Country fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CountryByUrlNameResponse>.Error(
					"Failed to fetch country",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
