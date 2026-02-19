using HotelAPI.Model.Region;

namespace HotelAPI.DAL.Interfaces
{
	public interface IRegionRepository
	{
		Task<IEnumerable<RegionsByCountryResponse>> GetRegionsByCountryAsync(int countryId);
	}
}
