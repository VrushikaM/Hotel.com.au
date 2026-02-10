using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Country;

namespace HotelAPI.BAL.Services
{
	public class CountryService(ICountryRepository _countryRepository, ICacheService _cache) : ICountryService
	{
		private const string COUNTRY_LIST_CACHE_KEY = "country:list";

		public async Task<ResponseResult<IEnumerable<CountryListResponse>>> GetCountryListAsync()
		{
			try
			{
				var data = await _cache.GetOrCreateAsync(
					cacheKey: COUNTRY_LIST_CACHE_KEY,
					factory: () => _countryRepository.GetCountryListAsync(),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(5)
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

		public async Task<ResponseResult<CountryByUrlResponse>> GetCountryByUrlAsync(string urlName, string? alphabet)
		{
			try
			{

				var data = await _cache.GetOrCreateAsync(
					cacheKey: $"COUNTRY_URL_{urlName}_{alphabet}",
					factory: () => _countryRepository.GetCountryByUrlAsync(urlName, alphabet),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				if (data == null)
				{
					return ResponseHelper<CountryByUrlResponse>.Error(
						"No country found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<CountryByUrlResponse>.Success(
					"Country fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CountryByUrlResponse>.Error(
					"Failed to fetch country",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
