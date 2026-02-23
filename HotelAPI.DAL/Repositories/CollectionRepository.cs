using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;

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
				"CollectionContent_Save",
				parameters
			);
		}

		public async Task<CollectionContentResponse?> GetAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", collectionId);

			return await _sqlHelper.QueryFirstOrDefaultAsync<CollectionContentResponse>(
				"CollectionContent_Get",
				parameters
			);
		}
		public async Task<IEnumerable<CollectionContentHistoryResponse>> GetHistoryAsync(int collectionId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@CollectionId", collectionId);

			return await _sqlHelper.QueryAsync<CollectionContentHistoryResponse>(
				"CollectionContent_GetHistory",
				parameters
			);
		}
	}
}
