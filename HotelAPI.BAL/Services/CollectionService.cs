using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;

namespace HotelAPI.BAL.Services
{
	public class CollectionService(
		ICollectionRepository _collectionRepository,
		ICacheService _cache
	) : ICollectionService
	{
		private const string COLLECTION_LIST_CACHE_KEY = "collection:list";
		private const string COLLECTION_CONTENT_CACHE_KEY = "collection:content";
		private const string COLLECTION_HISTORY_CACHE_KEY = "collection:history";

		#region Collection List

		public async Task<ResponseResult<IEnumerable<CollectionListResponse>>> GetCollectionListAsync(
			string? status,
			int? countryId,
			int? regionId,
			int? cityId)
		{
			try
			{
				var cacheKey =
					$"{COLLECTION_LIST_CACHE_KEY}:" +
					$"{status ?? "all"}:" +
					$"{countryId ?? 0}:" +
					$"{regionId ?? 0}:" +
					$"{cityId ?? 0}";

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetCollectionListAsync(status, countryId, regionId, cityId),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(5)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<CollectionListResponse>>.Error(
						"No collections found",
						statusCode : StatusCode.NOT_FOUND
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

		#endregion

		#region Upsert Collection

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

				// 🔥 Clear collection list cache after insert/update
				 _cache.Remove(COLLECTION_LIST_CACHE_KEY);

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

		#endregion

		#region Save Content

		public async Task<ResponseResult<bool>> SaveAsync(CollectionContentRequest request)
		{
			try
			{
				await _collectionRepository.SaveAsync(request);

				// 🔥 Clear content & history cache after save
				 _cache.Remove($"{COLLECTION_CONTENT_CACHE_KEY}:{request.CollectionId}");
				 _cache.Remove($"{COLLECTION_HISTORY_CACHE_KEY}:{request.CollectionId}");

				return ResponseHelper<bool>.Success(
					"Content saved successfully",
					true
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<bool>.Error(
					"Error while saving content",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		#endregion

		#region Get Content

		public async Task<ResponseResult<CollectionContentResponse?>> GetAsync(int collectionId)
		{
			try
			{
				var cacheKey = $"{COLLECTION_CONTENT_CACHE_KEY}:{collectionId}";

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetAsync(collectionId),
					TimeSpan.FromMinutes(30),
					TimeSpan.FromMinutes(10)
				);

				return ResponseHelper<CollectionContentResponse?>.Success(
					"Content fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CollectionContentResponse?>.Error(
					"Error fetching content",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		#endregion

		#region Get History

		public async Task<ResponseResult<IEnumerable<CollectionContentHistoryResponse>>> GetHistoryAsync(int collectionId)
		{
			try
			{
				var cacheKey = $"{COLLECTION_HISTORY_CACHE_KEY}:{collectionId}";

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetHistoryAsync(collectionId),
					TimeSpan.FromMinutes(30),
					TimeSpan.FromMinutes(10)
				);

				return ResponseHelper<IEnumerable<CollectionContentHistoryResponse>>.Success(
					"History fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<CollectionContentHistoryResponse>>.Error(
					"Error fetching history",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		#endregion
	}
}