using HotelAPI.Model.District;

namespace HotelAPI.DAL.Interfaces
{
	public interface IDistrictRepository
	{
		Task<IEnumerable<DistrictsByCityResponse>> GetDistrictsByCityAsync(int cityId, string? searchTerm);
	}
}
