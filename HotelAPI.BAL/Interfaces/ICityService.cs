using HotelAPI.Common.Helper;
using HotelAPI.Model.City;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICityService
	{
		Task<ResponseResult<IEnumerable<CitiesByUrlRegistryResponse>>> GetCitiesByUrlRegistryAsync(int registryId);
	}
}
