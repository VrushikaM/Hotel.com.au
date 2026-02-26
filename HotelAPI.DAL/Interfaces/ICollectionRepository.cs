using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionCuration;
using HotelAPI.Model.Collection.CollectionRule;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICollectionRepository
	{
		Task<IEnumerable<CollectionListResponse>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId);
		Task<int> UpsertCollectionAsync(CollectionUpsertRequest request);
		Task UpsertContentAsync(CollectionContentRequest request);
		Task<CollectionContentResponse?> GetContentAsync(int collectionId);
		Task<IEnumerable<CollectionContentHistoryResponse>> GetContentHistoryAsync(int collectionId);
		Task<IEnumerable<int>> UpsertRulesAsync(int collectionId, string rulesJson);
		Task<CollectionRuleResponse?> GetRulesByIdAsync(int collectionId);
		Task<int> ChangeStatusAsync(int collectionId, string action);
		Task<CollectionCurationResponse?> UpsertCurationsAsync(CollectionCurationRequest request);
		Task<CurationByIdResponse?> GetCurationsByIdAsync(int collectionId);
		Task<CollectionByIdResponse?> GetCollectionAsync(int collectionId);
	}
}
