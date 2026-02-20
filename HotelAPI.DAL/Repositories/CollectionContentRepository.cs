using Dapper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection.CollectionContent;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.DAL.Repositories
{
	public class CollectionContentRepository(ISqlHelper _sqlHelper)
	: ICollectionContentRepository
	{
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
