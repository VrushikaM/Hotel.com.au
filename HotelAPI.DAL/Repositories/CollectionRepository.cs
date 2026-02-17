using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection;

namespace HotelAPI.DAL.Repositories
{
	public class CollectionRepository(ISqlHelper _sqlHelper) : ICollectionRepository
	{
		#region GetCollectionListAsync
		public async Task<IEnumerable<CollectionListResponse>> GetCollectionListAsync(string? status, int? geoNodeId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@Status", status);
			parameters.Add("@GeoNodeId", geoNodeId);

			return await _sqlHelper.QueryAsync<CollectionListResponse>(
				StoredProcedure.GetCollectionList, parameters
			);
		}
		#endregion

		#region UpsertCollectionAsync
		public async Task<int> UpsertCollectionAsync(CollectionUpsertRequest request)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", request.CollectionId);
			parameters.Add("@CollectionJson", request.CollectionJson);
			parameters.Add("@RulesJson", request.RulesJson);
			parameters.Add("@PinnedJson", request.PinnedJson);
			parameters.Add("@ExcludeJson", request.ExcludeJson);
			parameters.Add("@ChangedBy", request.ChangedBy);
			parameters.Add("@IsDebug", request.IsDebug);

			var result = await _sqlHelper.QueryFirstOrDefaultAsync<CollectionUpsertResponse>(
				StoredProcedure.UpsertCollection,
				parameters
			);

			return result?.CollectionId ?? 0;
		}
		#endregion
	}
}
