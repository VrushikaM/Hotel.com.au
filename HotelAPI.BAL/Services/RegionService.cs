using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Region;

namespace HotelAPI.BAL.Services
{
	public class RegionService(IRegionRepository _regionRepository, ICacheService _cache) : IRegionService
	{
		private const string REGION_BY_COUNTRY_LIST_CACHE_KEY = "regionsByCountry:list";

		public async Task<ResponseResult<IEnumerable<RegionsByCountryResponse>>> GetRegionsByCountryAsync(int countryId)
		{
			try
			{

				var data = await _cache.GetOrCreateAsync(
					cacheKey: $"{REGION_BY_COUNTRY_LIST_CACHE_KEY}_{countryId}",
					factory: () => _regionRepository.GetRegionsByCountryAsync(countryId),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<RegionsByCountryResponse>>.Error(
						"No regions found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

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