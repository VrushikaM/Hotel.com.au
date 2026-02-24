using Dapper;
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
		public async Task<IEnumerable<CollectionListResponse>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@Status", status);
			parameters.Add("@CountryId", countryId);
			parameters.Add("@RegionId", regionId);
			parameters.Add("@CityId", cityId);

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

		#region SaveAsync
		public async Task SaveAsync(CollectionContentRequest request)
		{
			var parameters = new DynamicParameters();

			parameters.Add("@CollectionId", request.CollectionId);
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
				StoredProcedure.SaveCollectionContent,
				parameters
			);
		}
		#endregion

		#region GetAsync
		public async Task<CollectionContentResponse?> GetAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", collectionId);

			return await _sqlHelper.QueryFirstOrDefaultAsync<CollectionContentResponse>(
				StoredProcedure.GetCollectionContent,
				parameters
			);
		}
		#endregion

		#region GetHistoryAsync
		public async Task<IEnumerable<CollectionContentHistoryResponse>> GetHistoryAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", collectionId);

			return await _sqlHelper.QueryAsync<CollectionContentHistoryResponse>(
				StoredProcedure.GetCollectionContentHistory,
				parameters
			);
		}
		#endregion

		#region SaveRuleAsync
		public async Task<int> SaveRuleAsync(CollectionRuleRequest request)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@RuleID", request.RuleId);
			parameters.Add("@CollectionID", request.CollectionId);
			parameters.Add("@Field", request.Field);
			parameters.Add("@Operator", request.Operator);
			parameters.Add("@Value", request.Value);
			parameters.Add("@LogicalGroup", request.LogicalGroup);

			return await _sqlHelper.QueryFirstOrDefaultAsync<int>(
				StoredProcedure.UpsertCollectionRules,
				parameters
			);
		}
		#endregion

		#region GetRuleByIdAsync
		public async Task<CollectionRuleResponse?> GetRuleByIdAsync(int ruleId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@RuleID", ruleId);

			return await _sqlHelper.QueryFirstOrDefaultAsync<CollectionRuleResponse>(
				StoredProcedure.GetCollectionRules,
				parameters
			);
		}
		#endregion

		#region ChangeStatusAsync
		public async Task<long> ChangeStatusAsync(long collectionId, string action)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", collectionId);
			parameters.Add("@Action", action);

			return await _sqlHelper.QueryFirstOrDefaultAsync<long>(
				StoredProcedure.ChangeCollectionStatus,
				parameters
			);
		}
		#endregion

		#region	SaveCurationAsync
		public async Task<CollectionCurationResponse?> SaveCurationAsync(CollectionCurationRequest request)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionID", request.CollectionId);
			parameters.Add("@PinnedJson", request.PinnedJson);
			parameters.Add("@ExcludeJson", request.ExcludeJson);

			return await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.UpsertCollectionCuration,
				async multi =>
				{
					// Result set 1 → CollectionID (from SP)
					var collectionResult = (await multi.ReadAsync<long>()).FirstOrDefault();
					if (collectionResult == 0)
						return null;

					// Result set 2 → Exclusion IDs
					var exclusionIds = (await multi.ReadAsync<long>()).ToList();

					return new CollectionCurationResponse
					{
						PinnedHotels = new PinnedHotelsResponse
						{
							CollectionId = collectionResult
						},
						ExcludedHotels = new ExcludedHotelsResponse
						{
							ExclusionIds = exclusionIds
						}
					};
				},
				parameters
			);
		}
		#endregion
	}
}
