using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Region;

namespace HotelAPI.BAL.Services
{
	public class RegionService(IRegionRepository _regionRepository, ICacheService _cache) : IRegionService
	{
		public async Task<ResponseResult<IEnumerable<RegionsByCountryResponse>>> GetRegionsByCountryAsync(int countryId, string? searchTerm)
		{
			var cacheKey = CacheKeyBuilder.RegionsByCountry(countryId,searchTerm);

			try
			{
				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _regionRepository.GetRegionsByCountryAsync(countryId,searchTerm),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				var data = result ?? Enumerable.Empty<RegionsByCountryResponse>();

				return ResponseHelper<IEnumerable<RegionsByCountryResponse>>.Success(
					"Region list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<RegionsByCountryResponse>>.Error(
					"Failed to fetch regions",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}