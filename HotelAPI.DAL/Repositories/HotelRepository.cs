using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Hotel;

namespace HotelAPI.DAL.Repositories
{
	public class HotelRepository(ISqlHelper _sqlHelper) : IHotelRepository
	{
		#region GetHotelsByCityAsync
		public async Task<IEnumerable<HotelsByCityResponse>> GetHotelsByCityAsync(int? cityId, string? search)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CityId", cityId);
			parameters.Add("@Search",  search);

			return await _sqlHelper.QueryAsync<HotelsByCityResponse>(
				StoredProcedure.GetHotelsByCity,
				parameters
			);
		}
		#endregion
	}
}
