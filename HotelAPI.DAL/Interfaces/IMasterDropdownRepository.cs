using HotelAPI.Model.MasterDropdowns;

namespace HotelAPI.DAL.Interfaces
{
	public interface IMasterDropdownRepository
	{
		Task<MasterDropdownsResponse> GetMasterDropdownsAsync();
	}
}