using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Region;

namespace HotelAPI.DAL.Repositories
{
	public class RegionRepository(ISqlHelper _sqlHelper) : IRegionRepository
	{
		#region GetRegionsByCountryAsync
		public async Task<IEnumerable<RegionsByCountryResponse>> GetRegionsByCountryAsync(int countryId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CountryId", countryId);

			return await _sqlHelper.QueryAsync<RegionsByCountryResponse>(
				StoredProcedure.GetRegionsByCountry,
				parameters
			);
		}
		#endregion
	}
}
