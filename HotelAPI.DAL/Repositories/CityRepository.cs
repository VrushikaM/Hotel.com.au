using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.City;

namespace HotelAPI.DAL.Repositories
{
	public class CityRepository(ISqlHelper _sqlHelper) : ICityRepository
	{
		#region GetCitiesByCountryOrRegionAsync
		public async Task<IEnumerable<CitiesByCountryOrRegionResponse>> GetCitiesByCountryOrRegionAsync(int countryId, int? regionId, string? searchTerm)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CountryId", countryId);
			parameters.Add("@RegionId", regionId);
			parameters.Add("@SearchTerm", searchTerm);

			return await _sqlHelper.QueryAsync<CitiesByCountryOrRegionResponse>(
				StoredProcedure.GetCitiesByCountryOrRegion,
				parameters
			);
		}
		#endregion
	}
}
