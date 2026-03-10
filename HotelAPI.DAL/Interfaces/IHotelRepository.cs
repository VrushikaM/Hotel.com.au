using HotelAPI.Model.Hotel;

namespace HotelAPI.DAL.Interfaces
{
	public interface IHotelRepository
	{
		Task<IEnumerable<HotelsByGeoNodeResponse>> GetHotelsByGeoNode(string geoNodeType, int geoNodeId, string? searchTerm, int maxCount);
	}
}
