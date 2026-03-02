using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.District;

namespace HotelAPI.BAL.Services
{
	public class DistrictService(IDistrictRepository _districtRepository, ICacheService _cache) : IDistrictService
	{
		public async Task<ResponseResult<IEnumerable<DistrictsByCityResponse>>> GetDistrictsByCityAsync(int cityId, string? searchTerm)
		{
			var cacheKey = CacheKeyBuilder.DistrictsByCity(cityId, searchTerm);

			try
			{
				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _districtRepository.GetDistrictsByCityAsync(cityId, searchTerm),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				var data = result ?? Enumerable.Empty<DistrictsByCityResponse>();

				return ResponseHelper<IEnumerable<DistrictsByCityResponse>>.Success(
					"District list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<DistrictsByCityResponse>>.Error(
					"Failed to fetch districts",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}