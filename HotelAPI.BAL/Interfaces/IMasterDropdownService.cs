using HotelAPI.Common.Helper;
using HotelAPI.Model.MasterDropdowns;

namespace HotelAPI.BAL.Interfaces
{
	public interface IMasterDropdownService
	{
		Task<ResponseResult<MasterDropdownsResponse>> GetMasterDropdownsAsync();
	}
}
