using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionRule;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICollectionRepository
	{
		Task<IEnumerable<CollectionListResponse>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId);
		Task<int> UpsertCollectionAsync(CollectionUpsertRequest request);
		Task SaveAsync(CollectionContentRequest request);
		Task<CollectionContentResponse?> GetAsync(int collectionId);
		Task<IEnumerable<CollectionContentHistoryResponse>> GetHistoryAsync(int collectionId);
		Task<int> SaveRuleAsync(CollectionRuleRequest request);
		Task<CollectionRuleResponse?> GetRuleByIdAsync(int ruleId);
		Task<long> ChangeStatusAsync(long collectionId, string action);
	}
}
