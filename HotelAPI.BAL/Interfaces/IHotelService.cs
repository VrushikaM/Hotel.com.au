using HotelAPI.Common.Helper;
using HotelAPI.Model.Hotel;

namespace HotelAPI.BAL.Interfaces
{
	public interface IHotelService
	{
		Task<ResponseResult<IEnumerable<HotelsByGeoNodeResponse>>> GetHotelsByGeoNodeAsync(string geoNodeType, int geoNodeId, string? searchTerm, int maxCount);
	}
}
