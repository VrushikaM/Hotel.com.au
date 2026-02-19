using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.City;

namespace HotelAPI.BAL.Services
{
	public class CityService(ICityRepository _cityRepository, ICacheService _cache) : ICityService
	{
		private const string CITY_BY_URLREGISTRY_LIST_CACHE_KEY = "citiesByUrlRegistry:list";

		public async Task<ResponseResult<IEnumerable<CitiesByUrlRegistryResponse>>> GetCitiesByUrlRegistryAsync(int registryId)
		{
			try
			{

				var data = await _cache.GetOrCreateAsync(
					cacheKey: $"{CITY_BY_URLREGISTRY_LIST_CACHE_KEY}_{registryId}",
					factory: () => _cityRepository.GetCitiesByUrlRegistryAsync(registryId),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<CitiesByUrlRegistryResponse>>.Error(
						"No cities found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<IEnumerable<CitiesByUrlRegistryResponse>>.Success(
					"City list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<CitiesByUrlRegistryResponse>>.Error(
					"Failed to fetch cities",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
