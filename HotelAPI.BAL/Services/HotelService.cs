using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Hotel;

namespace HotelAPI.BAL.Services
{
	public class HotelService(IHotelRepository _hotelRepository, ICacheService _cache) : IHotelService
	{
		private const string Hotel_BY_CITY_LIST_CACHE_KEY = "hotelsByCity:list";

		public async Task<ResponseResult<IEnumerable<HotelsByCityResponse>>> GetHotelsByCityAsync(int cityId)
		{
			try
			{

				var data = await _cache.GetOrCreateAsync(
					cacheKey: $"{Hotel_BY_CITY_LIST_CACHE_KEY}_{cityId}",
					factory: () => _hotelRepository.GetHotelsByCityAsync(cityId),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<HotelsByCityResponse>>.Error(
						"No hotels found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

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
