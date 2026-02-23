using HotelAPI.Common.Helper;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICollectionService
	{
		Task<ResponseResult<IEnumerable<CollectionListResponse>>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId);
		Task<ResponseResult<CollectionUpsertResponse>> UpsertCollectionAsync(CollectionUpsertRequest request);
		Task<ResponseResult<bool>> SaveAsync(CollectionContentRequest request);
		Task<ResponseResult<CollectionContentResponse?>> GetAsync(int collectionId);
		Task<ResponseResult<IEnumerable<CollectionContentHistoryResponse>>> GetHistoryAsync(int collectionId);
	}
}
