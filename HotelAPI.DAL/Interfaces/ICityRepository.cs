using HotelAPI.Model.City;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICityRepository
	{
		Task<IEnumerable<CitiesByCountryOrRegionResponse>> GetCitiesByCountryOrRegionAsync(int countryId, int? regionId, string? searchTerm);
	}
}
