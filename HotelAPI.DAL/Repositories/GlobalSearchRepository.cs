using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Search;

namespace HotelAPI.DAL.Repositories
{
	public class GlobalSearchRepository(ISqlHelper _sqlHelper) : IGlobalSearchRepository
	{
		#region SearchAsync
		public async Task<IEnumerable<GlobalSearchResponse>> SearchAsync(string searchText)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@SearchText", searchText);

			return await _sqlHelper.QueryAsync<GlobalSearchResponse>(StoredProcedure.GlobalSearch, parameters);
		}
		#endregion
	}
}
