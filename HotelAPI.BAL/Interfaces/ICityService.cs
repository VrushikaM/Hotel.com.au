using HotelAPI.Common.Helper;
using HotelAPI.Model.City;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICityService
	{
		Task<ResponseResult<IEnumerable<CitiesByCountryOrRegionResponse>>> GetCitiesByCountryOrRegionAsync(int countryId, int? regionId, string? searchTerm);
	}
}
