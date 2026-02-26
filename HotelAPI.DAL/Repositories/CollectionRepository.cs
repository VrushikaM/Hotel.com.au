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
			parameters.Add("@ChangedBy", request.ChangedBy);

			var result = await _sqlHelper.QueryFirstOrDefaultAsync<CollectionUpsertResponse>(
				StoredProcedure.UpsertCollection,
				parameters
			);

			return result?.CollectionId ?? 0;
		}
		#endregion

		#region UpsertContentAsync
		public async Task UpsertContentAsync(CollectionContentRequest request)
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
				StoredProcedure.UpsertCollectionContent,
				parameters
			);
		}
		#endregion

		#region GetContentAsync
		public async Task<CollectionContentResponse?> GetContentAsync(int collectionId)
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
		public async Task<IEnumerable<CollectionContentHistoryResponse>> GetContentHistoryAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", collectionId);

			return await _sqlHelper.QueryAsync<CollectionContentHistoryResponse>(
				StoredProcedure.GetCollectionContentHistory,
				parameters
			);
		}
		#endregion

		#region UpsertRulesAsync
		public async Task<IEnumerable<int>> UpsertRulesAsync(int collectionId, string rulesJson)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionID", collectionId);
			parameters.Add("@RulesJson", rulesJson);

			var ruleIds = await _sqlHelper.QueryAsync<int>(
				StoredProcedure.UpsertCollectionRules,
				parameters
			);

			return ruleIds;
		}
		#endregion

		#region GetRulesByIdAsync
		public async Task<CollectionRuleResponse?> GetRulesByIdAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionID", collectionId);

			var rules = await _sqlHelper.QueryAsync<Rules>(
				StoredProcedure.GetCollectionRules,
				parameters
			);
			return new CollectionRuleResponse
			{
				Rules = rules.ToList()
			};
		}
		#endregion

		#region ChangeStatusAsync
		public async Task<int> ChangeStatusAsync(int collectionId, string action)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", collectionId);
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
			parameters.Add("@CollectionID", request.CollectionId);
			parameters.Add("@PinnedJson", request.PinnedJson);
			parameters.Add("@ExcludeJson", request.ExcludeJson);

			return await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.UpsertCollectionCurations,
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

		#region GetCurationsByIdAsync
		public async Task<CurationByIdResponse?> GetCurationsByIdAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionID", collectionId);

			return await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.GetCollectionCurations,
				async multi =>
				{
					// Result set 1 → Pinned Hotels
					var pinnedHotels = (await multi.ReadAsync<PinnedHotelsByIdResponse>()).ToList();

					// Result set 2 → Excluded Hotels
					var excludedHotels = (await multi.ReadAsync<ExcludedHotelsByIdResponse>()).ToList();

					if (pinnedHotels.Count == 0 && excludedHotels.Count == 0)
						return null;

					return new CurationByIdResponse
					{
						PinnedHotels = pinnedHotels,
						ExcludedHotels = excludedHotels
					};
				},
				parameters
			);
		}
		#endregion

		#region GetCollectionAsync
		public async Task<CollectionByIdResponse?> GetCollectionAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", collectionId);

			return await _sqlHelper.QueryFirstOrDefaultAsync<CollectionByIdResponse>(
				StoredProcedure.GetCollectionById,
				parameters
			);
		}
		#endregion
	}
}
