using Dapper;
using HotelAPI.Common.Constants;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionCuration;
using HotelAPI.Model.Collection.CollectionRule;

namespace HotelAPI.DAL.Repositories
{
	public class CollectionRepository(ISqlHelper _sqlHelper) : ICollectionRepository
	{
		#region GetCollectionListAsync
		public async Task<CollectionListResponse> GetCollectionListAsync(string? status, string? geoNodeType, int? sourceId, int pageNumber, int pageSize)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@Status", status);
			parameters.Add("@GeoNodeType", geoNodeType);
			parameters.Add("@SourceId", sourceId);
			parameters.Add("@PageNumber", pageNumber);
			parameters.Add("@PageSize", pageSize);

			var result = await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.GetCollectionList,
				async multi =>
				{
					// First result set: TotalRecords
					var totalRecords = await multi.ReadFirstAsync<int>();

					// Second result set: Paged CollectionData
					var collections = (await multi.ReadAsync<CollectionData>()).ToList();

					return new CollectionListResponse
					{
						TotalRecords = totalRecords.ToString(),
						Collections = collections
					};
				},
				parameters
			);

			return result;
		}
		#endregion

		#region UpsertCollectionAsync
		public async Task<int> UpsertCollectionAsync(CollectionUpsertRequest request)
		{
			var parameters = new DynamicParameters();
			parameters.Add(DbParameters.CollectionId, request.CollectionId);
			parameters.Add("@CollectionJson", request.CollectionJson);
			parameters.Add("@ChangedBy", request.ChangedBy);

			var result = await _sqlHelper.QueryFirstOrDefaultAsync<CollectionUpsertResponse>(
				StoredProcedure.UpsertCollection,
				parameters
			);

			return result?.CollectionId ?? 0;
		}
		#endregion

		#region GetCollectionAsync
		public async Task<CollectionByIdResponse?> GetCollectionAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add(DbParameters.CollectionId, collectionId);

			return await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.GetCollectionById,
				async multi =>
				{
					var basic = await multi.ReadFirstOrDefaultAsync<BasicCollectionResponse>();
					var content = await multi.ReadFirstOrDefaultAsync<CollectionContentResponse>();
					var contentHistory = await multi.ReadFirstOrDefaultAsync<CollectionContentHistoryResponse>();
					var rules = (await multi.ReadAsync<Rules>()).ToList();
					var includedHotels = (await multi.ReadAsync<IncludedHotelsByIdResponse>()).ToList();
					var pinnedHotels = (await multi.ReadAsync<PinnedHotelsByIdResponse>()).ToList();
					var excludedHotels = (await multi.ReadAsync<ExcludedHotelsByIdResponse>()).ToList();
					var previewHotels = await GetCollectionPreviewHotelsAsync(collectionId);

					return new CollectionByIdResponse
					{
						BasicCollection = basic,
						CollectionContent = content,
						CollectionContentHistory = contentHistory,
						CollectionRules = new List<CollectionRuleResponse>
						{
							new CollectionRuleResponse { Rules = rules }
						},
						CollectionCuration = new List<CurationByIdResponse>
						{
							new CurationByIdResponse
							{
								IncludedHotels = includedHotels.Count() == 0 ? includedHotels : new List<IncludedHotelsByIdResponse>(),
								PinnedHotels = pinnedHotels.Count() == 0  ? pinnedHotels : new List<PinnedHotelsByIdResponse>(),
								ExcludedHotels = excludedHotels.Count() == 0 ? excludedHotels : new List<ExcludedHotelsByIdResponse>()
							}
						},
						CollectionPreviewHotels = previewHotels
					};
				},
				parameters
			);
		}
		#endregion

		#region UpsertContentAsync
		public async Task UpsertContentAsync(CollectionContentRequest request)
		{
			var parameters = new DynamicParameters();

			parameters.Add(DbParameters.CollectionId, request.CollectionId);
			parameters.Add("@Header", request.Header);
			parameters.Add("@MetaTitle", request.MetaTitle);
			parameters.Add("@MetaDescription", request.MetaDescription);
			parameters.Add("@IntroShortCopy", request.IntroShortCopy);
			parameters.Add("@IntroLongCopy", request.IntroLongCopy);
			parameters.Add("@HeroImageUrl", request.HeroImageUrl);
			parameters.Add("@Badge", request.Badge);
			parameters.Add("@FAQsJson", request.FAQsJson);
			parameters.Add("@UserId", 33);

			await _sqlHelper.ExecuteAsync(
				StoredProcedure.UpsertCollectionContent,
				parameters
			);
		}
		#endregion

		#region UpsertRulesAsync
		public async Task<IEnumerable<int>> UpsertRulesAsync(int collectionId, string rulesJson)
		{
			var parameters = new DynamicParameters();
			parameters.Add(DbParameters.CollectionId, collectionId);
			parameters.Add("@RulesJson", rulesJson);

			var ruleIds = await _sqlHelper.QueryAsync<int>(
				StoredProcedure.UpsertCollectionRules,
				parameters
			);

			return ruleIds;
		}
		#endregion

		#region ChangeStatusAsync
		public async Task<int> ChangeStatusAsync(int collectionId, string action)
		{
			var parameters = new DynamicParameters();
			parameters.Add(DbParameters.CollectionId, collectionId);
			parameters.Add("@Action", action);

			return await _sqlHelper.QueryFirstOrDefaultAsync<int>(
				StoredProcedure.ChangeCollectionStatus,
				parameters
			);
		}
		#endregion

		#region	UpsertCurationsAsync
		public async Task<CollectionCurationResponse?> UpsertCurationsAsync(CollectionCurationRequest request)
		{
			var parameters = new DynamicParameters();
			parameters.Add(DbParameters.CollectionId, request.CollectionId);
			parameters.Add("@IncludeJson", request.IncludeJson);
			parameters.Add("@PinnedJson", request.PinnedJson);
			parameters.Add("@ExcludeJson", request.ExcludeJson);

			var result = await _sqlHelper.QueryFirstOrDefaultAsync<CollectionCurationResponse>(
				StoredProcedure.UpsertCollectionCurations,
				parameters
			);

			return result;
		}
		#endregion

		#region CloneCollectionAsync
		public async Task<long> CloneCollectionAsync(long sourceCollectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@SourceCollectionId", sourceCollectionId);
			parameters.Add("@NewCollectionId", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);

			await _sqlHelper.ExecuteAsync(
				StoredProcedure.CloneCollection,
				parameters
			);

			return parameters.Get<long>("@NewCollectionId");
		}
		#endregion

		#region DeleteCollectionAsync
		public async Task<long> DeleteCollectionAsync(long collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add(DbParameters.CollectionId, collectionId);

			var result = await _sqlHelper.QueryFirstOrDefaultAsync<long>(
				StoredProcedure.CollectionDelete,
				parameters
			);

			return result;
		}
		#endregion

		#region GetCollectionPreviewHotelsAsync
		public async Task<List<CollectionPreviewHotelsResponse>> GetCollectionPreviewHotelsAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add(DbParameters.CollectionId, collectionId);

			var previewHotels = (await _sqlHelper.QueryAsync<CollectionPreviewHotelsResponse>(
				StoredProcedure.CollectionPreviewHotels,
				parameters
			)).ToList();

			return previewHotels;
		}
		#endregion
	}
}
