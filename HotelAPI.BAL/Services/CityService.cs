using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.City;

namespace HotelAPI.BAL.Services
{
	public class CityService(ICityRepository _cityRepository, ICacheService _cache) : ICityService
	{
		public async Task<ResponseResult<IEnumerable<CitiesByCountryOrRegionResponse>>> GetCitiesByCountryOrRegionAsync(int countryId, int? regionId, string? searchTerm)
		{
			var cacheKey = CacheKeyBuilder.CitiesByCountryOrRegion(countryId, regionId,searchTerm);

			try
			{
				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _cityRepository.GetCitiesByCountryOrRegionAsync(countryId, regionId,searchTerm),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				var data = result ?? Enumerable.Empty<CitiesByCountryOrRegionResponse>();

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
