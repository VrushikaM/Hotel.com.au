using HotelAPI.Model.Collection;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICollectionRepository
	{
		Task<IEnumerable<CollectionListResponse>> GetCollectionListAsync(string? status, int? geoNodeId);
		Task<int> UpsertCollectionAsync(CollectionUpsertRequest request);
	}
}
