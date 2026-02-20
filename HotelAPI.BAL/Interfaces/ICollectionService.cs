using HotelAPI.Common.Helper;
using HotelAPI.Model.Collection;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICollectionService
	{
		Task<ResponseResult<IEnumerable<CollectionListResponse>>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId);
		Task<ResponseResult<CollectionUpsertResponse>> UpsertCollectionAsync(CollectionUpsertRequest request);
	}
}
