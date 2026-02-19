using HotelAPI.Model.City;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICityRepository
	{
		Task<IEnumerable<CitiesByUrlRegistryResponse>> GetCitiesByUrlRegistryAsync(int registryId);
	}
}
