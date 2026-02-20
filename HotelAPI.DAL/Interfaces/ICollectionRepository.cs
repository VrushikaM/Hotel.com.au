using HotelAPI.Model.Collection;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICollectionRepository
	{
		Task<IEnumerable<CollectionListResponse>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId);
		Task<int> UpsertCollectionAsync(CollectionUpsertRequest request);
	}
}
