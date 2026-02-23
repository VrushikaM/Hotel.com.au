using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICollectionRepository
	{
		Task<IEnumerable<CollectionListResponse>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId);
		Task<int> UpsertCollectionAsync(CollectionUpsertRequest request);
		Task SaveAsync(CollectionContentRequest request);
		Task<CollectionContentResponse?> GetAsync(int collectionId);
		Task<IEnumerable<CollectionContentHistoryResponse>> GetHistoryAsync(int collectionId);
	}
}
