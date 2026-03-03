using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionCuration;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICollectionRepository
	{
		Task<CollectionListResponse> GetCollectionListAsync(string? status, string? geoNodeType, int? geoNodeId, int pageNumber, int pageSize);
		Task<int> UpsertCollectionAsync(CollectionUpsertRequest request);
		Task<CollectionByIdResponse?> GetCollectionAsync(int collectionId);
		Task UpsertContentAsync(CollectionContentRequest request);
		Task<IEnumerable<int>> UpsertRulesAsync(int collectionId, string rulesJson);
		Task<int> ChangeStatusAsync(int collectionId, string action);
		Task<CollectionCurationResponse?> UpsertCurationsAsync(CollectionCurationRequest request);
		Task<long> CloneCollectionAsync(long sourceCollectionId);
		Task<long> DeleteCollectionAsync(long collectionId);
	}
}
