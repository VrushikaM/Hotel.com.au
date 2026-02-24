using HotelAPI.Model.Search;

namespace HotelAPI.DAL.Interfaces
{
	public interface IGlobalSearchRepository
	{
		Task<IEnumerable<GlobalSearchResponse>> SearchAsync(string searchText);
	}
}
