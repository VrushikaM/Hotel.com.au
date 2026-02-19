using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.City;

namespace HotelAPI.BAL.Services
{
	public class CityService(ICityRepository _cityRepository, ICacheService _cache) : ICityService
	{
		private const string CITY_BY_COUNTRY_OR_REGION_LIST_CACHE_KEY = "citiesByCountryOrRegion:list";

		public async Task<ResponseResult<IEnumerable<CitiesByCountryOrRegionResponse>>> GetCitiesByCountryOrRegionAsync(int countryId, int? regionId)
		{
			try
			{
					var data = await _cache.GetOrCreateAsync(
					cacheKey: $"{CITY_BY_COUNTRY_OR_REGION_LIST_CACHE_KEY}_{countryId}_{regionId}",
					factory: () => _cityRepository.GetCitiesByCountryOrRegionAsync(countryId,regionId),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<CitiesByCountryOrRegionResponse>>.Error(
						"No cities found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<IEnumerable<CitiesByCountryOrRegionResponse>>.Success(
					"City list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<CitiesByCountryOrRegionResponse>>.Error(
					"Failed to fetch cities",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
