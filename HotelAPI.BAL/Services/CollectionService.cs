using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection;

namespace HotelAPI.BAL.Services
{
	public class CollectionService(ICollectionRepository _collectionRepository, ICacheService _cache) : ICollectionService
	{
		private const string COLLECTION_LIST_CACHE_KEY = "collection:list";

		public async Task<ResponseResult<IEnumerable<CollectionListResponse>>> GetCollectionListAsync(string? status, int? geoNodeId)
		{
			try
			{
				var data = await _cache.GetOrCreateAsync(
					cacheKey: $"{COLLECTION_LIST_CACHE_KEY}_{status}_{geoNodeId}",
					factory: () => _collectionRepository.GetCollectionListAsync(status, geoNodeId),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(5)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<CollectionListResponse>>.Error(
						"No collections found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<IEnumerable<CollectionListResponse>>.Success(
					"Collection list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<CollectionListResponse>>.Error(
					"Failed to fetch collection list",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<CollectionUpsertResponse>> UpsertCollectionAsync(CollectionUpsertRequest request)
		{
			try
			{
				var collectionId = await _collectionRepository.UpsertCollectionAsync(request);

				if (collectionId <= 0)
				{
					return ResponseHelper<CollectionUpsertResponse>.Error(
						"Failed to save collection",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				return ResponseHelper<CollectionUpsertResponse>.Success(
					request.CollectionId == null
						? "Collection created successfully"
						: "Collection updated successfully",
					new CollectionUpsertResponse
					{
						CollectionId = collectionId
					}
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CollectionUpsertResponse>.Error(
					"Error occurred while saving collection",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
