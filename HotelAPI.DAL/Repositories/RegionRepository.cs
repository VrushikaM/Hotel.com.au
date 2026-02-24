using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Region;

namespace HotelAPI.DAL.Repositories
{
	public class RegionRepository(ISqlHelper _sqlHelper) : IRegionRepository
	{
		#region GetRegionsByCountryAsync
		public async Task<IEnumerable<RegionsByCountryResponse>> GetRegionsByCountryAsync(int countryId, string? searchTerm)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CountryId", countryId);
			parameters.Add("@SearchTerm", searchTerm);

			return await _sqlHelper.QueryAsync<RegionsByCountryResponse>(
				StoredProcedure.GetRegionsByCountry,
				parameters
			);
		}
		#endregion
	}
}
