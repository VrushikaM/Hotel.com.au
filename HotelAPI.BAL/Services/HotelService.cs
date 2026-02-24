using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Hotel;

namespace HotelAPI.BAL.Services
{
	public class HotelService(IHotelRepository _hotelRepository, ICacheService _cache) : IHotelService
	{
		public async Task<ResponseResult<IEnumerable<HotelsByCityResponse>>> GetHotelsByCityAsync(int? cityId, string? search)
		{
			var cacheKey = CacheKeyBuilder.HotelsByCity(cityId, search);

			try
			{
				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					factory: () => _hotelRepository.GetHotelsByCityAsync(cityId, search),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				var data = result ?? Enumerable.Empty<HotelsByCityResponse>();

				return ResponseHelper<IEnumerable<HotelsByCityResponse>>.Success(
					"Hotel list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<HotelsByCityResponse>>.Error(
					"Failed to fetch hotels",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
