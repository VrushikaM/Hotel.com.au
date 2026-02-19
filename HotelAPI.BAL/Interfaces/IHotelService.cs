using HotelAPI.Common.Helper;
using HotelAPI.Model.Hotel;

namespace HotelAPI.BAL.Interfaces
{
	public interface IHotelService
	{
		Task<ResponseResult<IEnumerable<HotelsByCityResponse>>> GetHotelsByCityAsync(int? cityId, string? search);
	}
}
