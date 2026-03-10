using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Hotel;

namespace HotelAPI.DAL.Repositories
{
	public class HotelRepository(ISqlHelper _sqlHelper) : IHotelRepository
	{
		#region GetHotelsByGeoNodeAsync
		public async Task<IEnumerable<HotelsByGeoNodeResponse>> GetHotelsByGeoNode(string geoNodeType, int geoNodeId, string? searchTerm, int maxCount)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@GeoNodeType", geoNodeType);
			parameters.Add("@GeoNodeId", geoNodeId);
			parameters.Add("@SearchTerm", searchTerm);
			parameters.Add("@MaxCount", maxCount);

			return await _sqlHelper.QueryAsync<HotelsByGeoNodeResponse>(
				StoredProcedure.GetHotelsByGeoNode,
				parameters
			);
		}
		#endregion
	}
}
