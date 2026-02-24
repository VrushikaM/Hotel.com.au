using HotelAPI.Common.Helper;
using HotelAPI.Model.Search;

namespace HotelAPI.BAL.Interfaces
{
	public interface IGlobalSearchService
	{
		Task<ResponseResult<IEnumerable<GlobalSearchResponse>>> SearchAsync(string searchText);
	}
}
