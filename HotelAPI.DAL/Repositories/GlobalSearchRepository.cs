using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.DAL.Repositories
{
	public class GlobalSearchRepository(ISqlHelper sqlHelper) : IGlobalSearchRepository
	{
		public async Task<IEnumerable<GlobalSearchResponse>>SearchAsync(string searchText)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@SearchText", searchText);

			return await sqlHelper.QueryAsync<GlobalSearchResponse>(StoredProcedure.GlobalSearch,parameters);
		}
	}
}
