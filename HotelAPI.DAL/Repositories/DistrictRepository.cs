using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.District;

namespace HotelAPI.DAL.Repositories
{
	public class DistrictRepository(ISqlHelper _sqlHelper) : IDistrictRepository
	{
		#region GetDistrictsByCityAsync
		public async Task<IEnumerable<DistrictsByCityResponse>> GetDistrictsByCityAsync(int cityId, string? searchTerm)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CityId", cityId);
			parameters.Add("@SearchTerm", searchTerm);

			return await _sqlHelper.QueryAsync<DistrictsByCityResponse>(
				StoredProcedure.GetDistrictsByCity,
				parameters
			);
		}
		#endregion
	}
}
