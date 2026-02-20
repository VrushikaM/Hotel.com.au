using HotelAPI.Common.Helper;
using HotelAPI.Model.Collection.CollectionContent;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICollectionContentService
	{
		Task<ResponseResult<bool>> SaveAsync(CollectionContentRequest request);
		Task<ResponseResult<CollectionContentResponse?>> GetAsync(int collectionId);
		Task<ResponseResult<IEnumerable<CollectionContentHistoryResponse>>> GetHistoryAsync(int collectionId);
	}
}
