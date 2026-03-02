using HotelAPI.Common.Helper;
using HotelAPI.Model.District;

namespace HotelAPI.BAL.Interfaces
{
	public interface IDistrictService
	{
		Task<ResponseResult<IEnumerable<DistrictsByCityResponse>>> GetDistrictsByCityAsync(int cityId, string? searchTerm);
	}
}
