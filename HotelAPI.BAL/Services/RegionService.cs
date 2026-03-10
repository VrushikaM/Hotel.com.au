using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Region;
using System.Net;

namespace HotelAPI.BAL.Services
{
	public class RegionService(IRegionRepository _regionRepository, ICacheService _cache) : IRegionService
	{
		public async Task<ResponseResult<IEnumerable<RegionsByCountryResponse>>> GetRegionsByCountryAsync(int countryId, string? searchTerm)
		{
			var cacheKey = CacheKeyBuilder.RegionsByCountry(countryId, searchTerm);

			try
			{
				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _regionRepository.GetRegionsByCountryAsync(countryId, searchTerm),
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

		public async Task<ResponseResult<IEnumerable<RegionsByUrlResponse>>> GetRegionByUrlAsync(string urlName)
		{
			//var normalizedUrl = urlName.Trim().ToLowerInvariant();
			urlName = WebUtility.UrlDecode(urlName);
			var cacheKey = CacheKeyBuilder.RegionByUrl(urlName);

			try
			{
				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _regionRepository.GetRegionByUrlAsync(urlName),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				var data = result ?? Enumerable.Empty<RegionsByUrlResponse>();

				return ResponseHelper<IEnumerable<RegionsByUrlResponse>>.Success(
					"Region fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<RegionsByUrlResponse>>.Error(
					"Failed to fetch region",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
