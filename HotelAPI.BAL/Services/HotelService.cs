using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Hotel;

namespace HotelAPI.BAL.Services
{
	public class HotelService(IHotelRepository _hotelRepository, ICacheService _cache) : IHotelService
	{
		public async Task<ResponseResult<IEnumerable<HotelsByGeoNodeResponse>>> GetHotelsByGeoNodeAsync(string geoNodeType, int geoNodeId, string? searchTerm, int maxCount)
		{
			var cacheKey = CacheKeyBuilder.HotelsByGeoNode(geoNodeType, geoNodeId, searchTerm, maxCount);

			try
			{
				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _hotelRepository.GetHotelsByGeoNode(geoNodeType, geoNodeId, searchTerm, maxCount),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				var data = result ?? Enumerable.Empty<HotelsByGeoNodeResponse>();

				return ResponseHelper<IEnumerable<HotelsByGeoNodeResponse>>.Success(
					"Hotel list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<HotelsByGeoNodeResponse>>.Error(
					"Failed to fetch hotels",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
