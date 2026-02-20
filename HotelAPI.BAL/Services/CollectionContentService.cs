using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection.CollectionContent;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.BAL.Services
{
	public class CollectionContentService(ICollectionContentRepository _repository)
	: ICollectionContentService
	{
		public async Task<ResponseResult<bool>> SaveAsync(CollectionContentRequest request)
		{
			try
			{
				await _repository.SaveAsync(request);

				return ResponseHelper<bool>.Success(
					"Content saved successfully",
					true
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<bool>.Error(
					"Error while saving content",
					exception: ex
				);
			}
		}

		public async Task<ResponseResult<CollectionContentResponse?>> GetAsync(int collectionId)
		{
			try
			{
				var data = await _repository.GetAsync(collectionId);

				return ResponseHelper<CollectionContentResponse?>.Success(
					"Content fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CollectionContentResponse?>.Error(
					"Error fetching content",
					exception: ex
				);
			}
		}

		public async Task<ResponseResult<IEnumerable<CollectionContentHistoryResponse>>> GetHistoryAsync(int collectionId)
		{
			try
			{
				var data = await _repository.GetHistoryAsync(collectionId);

				return ResponseHelper<IEnumerable<CollectionContentHistoryResponse>>.Success(
					"History fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<CollectionContentHistoryResponse>>.Error(
					"Error fetching history",
					 exception: ex

				);
			}
		}
	}
}