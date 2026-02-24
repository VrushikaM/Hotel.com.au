using HotelAPI.Common.Helper;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionRule;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICollectionService
	{
		Task<ResponseResult<IEnumerable<CollectionListResponse>>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId);
		Task<ResponseResult<CollectionUpsertResponse>> UpsertCollectionAsync(CollectionUpsertRequest request);
		Task<ResponseResult<bool>> SaveAsync(CollectionContentRequest request);
		Task<ResponseResult<CollectionContentResponse?>> GetAsync(int collectionId);
		Task<ResponseResult<IEnumerable<CollectionContentHistoryResponse>>> GetHistoryAsync(int collectionId);
		Task<ResponseResult<int>> SaveRuleAsync(CollectionRuleRequest request);
		Task<ResponseResult<CollectionRuleResponse?>> GetRuleByIdAsync(int ruleId);
		Task<ResponseResult<long>> ChangeStatusAsync(long collectionId, string action);
	}
}
