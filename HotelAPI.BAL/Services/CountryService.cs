using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Country;

namespace HotelAPI.BAL.Services
{
	public class CountryService : ICountryService
	{
		private readonly ICountryRepository _countryRepository;
		private readonly ICacheService _cache;

		private const string COUNTRY_LIST_CACHE_KEY = "COUNTRY_LIST";

		public CountryService(
			ICountryRepository countryRepository,
			ICacheService cache)
		{
			_countryRepository = countryRepository;
			_cache = cache;
		}

		public async Task<ResponseResult<IEnumerable<CountryListResponse>>> GetCountryListAsync()
		{
			try
			{
				var data = await _cache.GetOrCreateAsync(
					COUNTRY_LIST_CACHE_KEY,
					async () => await _countryRepository.GetCountryListAsync(),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(5)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<CountryListResponse>>.Error(
						"No countries found",
						statusCode: StatusCode.NOT_FOUND
					);
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

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					async () => await _countryRepository.GetCountryByUrlNameAsync(urlName, alphabet),
					TimeSpan.FromMinutes(10)
				);

				if (data == null)
				{
					return ResponseHelper<CountryByUrlNameResponse>.Error(
						"No country found",
						statusCode: StatusCode.NOT_FOUND
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
