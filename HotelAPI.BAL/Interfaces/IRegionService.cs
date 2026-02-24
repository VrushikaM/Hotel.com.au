using HotelAPI.Common.Helper;
using HotelAPI.Model.Region;

namespace HotelAPI.BAL.Interfaces
{
	public interface IRegionService
	{
		Task<ResponseResult<IEnumerable<RegionsByCountryResponse>>> GetRegionsByCountryAsync(int countryId);
	}
}
