using HotelAPI.Model.Hotel;

namespace HotelAPI.DAL.Interfaces
{
	public interface IHotelRepository
	{
		Task<IEnumerable<HotelsByCityResponse>> GetHotelsByCityAsync(int? cityId, string? searchTerm);
	}
}
