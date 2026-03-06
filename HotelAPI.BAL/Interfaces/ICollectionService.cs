using HotelAPI.Common.Helper;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionCuration;
using HotelAPI.Model.Collection.CollectionRule;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICollectionService
	{
		Task<ResponseResult<CollectionListResponse>> GetCollectionListAsync(string? status, string? geoNodeType, int? sourceId, int pageNumber, int pageSize);
		Task<ResponseResult<CollectionUpsertResponse>> UpsertCollectionAsync(CollectionUpsertRequest request);
		Task<ResponseResult<CollectionByIdResponse?>> GetCollectionAsync(int collectionId);
		Task<ResponseResult<bool>> UpsertContentAsync(CollectionContentRequest request);
		Task<ResponseResult<IEnumerable<int>>> UpsertRulesAsync(CollectionRuleRequest request);
		Task<ResponseResult<int>> ChangeStatusAsync(int collectionId, string action);
		Task<ResponseResult<CollectionCurationResponse>> UpsertCurationsAsync(CollectionCurationRequest request);
		Task<ResponseResult<long>> CloneCollectionAsync(int sourceCollectionId);
		Task<ResponseResult<long>> DeleteCollectionAsync(int collectionId);
		Task<ResponseResult<List<CollectionPreviewHotelsResponse>>> GetCollectionPreviewHotelsAsync(int collectionId);
	}
}
