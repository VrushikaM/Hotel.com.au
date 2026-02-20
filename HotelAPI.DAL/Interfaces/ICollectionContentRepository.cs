using HotelAPI.Model.Collection.CollectionContent;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICollectionContentRepository
	{
		Task SaveAsync(CollectionContentRequest request);
		Task<CollectionContentResponse?> GetAsync(int collectionId);
		Task<IEnumerable<CollectionContentHistoryResponse>> GetHistoryAsync(int collectionId);
	}
}
