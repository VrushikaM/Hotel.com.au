using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionCuration;
using HotelAPI.Model.Collection.CollectionRule;

namespace HotelAPI.BAL.Services
{
	public class CollectionService(ICollectionRepository _collectionRepository, ICacheService _cache) : ICollectionService
	{
		private const string COLLECTION_LIST_CACHE_KEY = "collection:list";

		public async Task<ResponseResult<CollectionListResponse>> GetCollectionListAsync(string? status, string? geoNodeType, int? sourceId, int pageNumber, int pageSize)
		{
			try
			{
				var version = await _cache.GetOrCreateAsync(
					COLLECTION_LIST_CACHE_KEY,
					() => Task.FromResult(Guid.NewGuid().ToString()),
					TimeSpan.FromHours(1),
					TimeSpan.FromHours(1)
				);

				var cacheKey = $"{CacheKeyBuilder.CollectionList(status, geoNodeType, sourceId, pageNumber, pageSize)}:{version}";

				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetCollectionListAsync(status, geoNodeType, sourceId, pageNumber, pageSize),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(10)
				);

				var data = result ?? new CollectionListResponse
				{
					TotalRecords = "0",
					Collections = new List<CollectionData>()
				};

				return ResponseHelper<CollectionListResponse>.Success(
					"Collection list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CollectionListResponse>.Error(
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
				if (request == null)
				{
					return ResponseHelper<CollectionUpsertResponse>.Error(
						"Request body cannot be null",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				if (string.IsNullOrWhiteSpace(request.CollectionJson))
				{
					return ResponseHelper<CollectionUpsertResponse>.Error(
						"CollectionJson is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}
				if (string.IsNullOrWhiteSpace(request.ChangedBy))
				{
					return ResponseHelper<CollectionUpsertResponse>.Error(
						"ChangedBy is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var collectionId = await _collectionRepository.UpsertCollectionAsync(request);

				if (collectionId <= 0)
				{
					return ResponseHelper<CollectionUpsertResponse>.Error(
						"Slug already exists.",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				// 🔥 Clear collection list cache after insert/update
				_cache.Remove(COLLECTION_LIST_CACHE_KEY);
				_cache.Remove(CacheKeyBuilder.CollectionById(collectionId));
				_cache.Remove(CacheKeyBuilder.CollectionPreviewHotels(collectionId));

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

		public async Task<ResponseResult<CollectionByIdResponse?>> GetCollectionAsync(int collectionId)
		{
			try
			{
				if (collectionId <= 0)
				{
					return ResponseHelper<CollectionByIdResponse?>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var cacheKey = CacheKeyBuilder.CollectionById(collectionId);

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetCollectionAsync(collectionId),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(10)
				);

				if (data == null)
				{
					return ResponseHelper<CollectionByIdResponse?>.Error(
						"Collection not found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<CollectionByIdResponse?>.Success(
					"Collection fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CollectionByIdResponse?>.Error(
					"Error fetching collection",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<bool>> UpsertContentAsync(CollectionContentRequest request)
		{
			try
			{
				if (request == null)
				{
					return ResponseHelper<bool>.Error(
						"Request body cannot be null",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				if (request.CollectionId <= 0)
				{
					return ResponseHelper<bool>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				if (string.IsNullOrWhiteSpace(request.Header))
				{
					return ResponseHelper<bool>.Error(
						"Header is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				await _collectionRepository.UpsertContentAsync(request);

				// 🔥 Clear content & history cache after save
				_cache.Remove(CacheKeyBuilder.CollectionById(request.CollectionId));
				_cache.Remove(CacheKeyBuilder.CollectionPreviewHotels(request.CollectionId));

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

		public async Task<ResponseResult<IEnumerable<int>>> UpsertRulesAsync(CollectionRuleRequest request)
		{
			try
			{
				if (request == null || request.RulesJson == null || request.RulesJson.Length == 0)
				{
					return ResponseHelper<IEnumerable<int>>.Error(
						"Rules list cannot be empty",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				// Call repository with JSON string and CollectionId
				var affectedRuleIds = await _collectionRepository.UpsertRulesAsync(request.CollectionId, request.RulesJson);

				if (affectedRuleIds == null || !affectedRuleIds.Any())
				{
					return ResponseHelper<IEnumerable<int>>.Error(
						"Failed to save rules",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				if (affectedRuleIds.Count() == 1 && affectedRuleIds.First() == 0)
				{
					return ResponseHelper<IEnumerable<int>>.Error(
						"Cannot exceed 8 rules per collection",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				_cache.Remove(CacheKeyBuilder.CollectionById(request.CollectionId));
				_cache.Remove(CacheKeyBuilder.CollectionPreviewHotels(request.CollectionId));

				return ResponseHelper<IEnumerable<int>>.Success(
					"Rules saved successfully",
					affectedRuleIds
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<int>>.Error(
					"Error while saving rules",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<int>> ChangeStatusAsync(int collectionId, string action)
		{
			try
			{
				if (collectionId <= 0)
				{
					return ResponseHelper<int>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				if (string.IsNullOrWhiteSpace(action))
				{
					return ResponseHelper<int>.Error(
						"Action is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var normalizedAction = action.Trim().ToLowerInvariant();

				if (normalizedAction != "draft" && normalizedAction != "publish")
				{
					return ResponseHelper<int>.Error(
						"Action must be either 'Draft' or 'Publish'",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var result = await _collectionRepository.ChangeStatusAsync(collectionId, normalizedAction);

				if (result <= 0)
				{
					return ResponseHelper<int>.Error(
						"Failed to change collection status",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				// 🔥 Clear collection list cache after status change
				_cache.Remove(COLLECTION_LIST_CACHE_KEY);

				return ResponseHelper<int>.Success(
					normalizedAction == "publish"
						? "Collection published successfully"
						: "Collection moved to draft successfully",
					result
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<int>.Error(
					"Error while changing collection status",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<CollectionCurationResponse>> UpsertCurationsAsync(CollectionCurationRequest request)
		{
			try
			{
				if (request == null)
				{
					return ResponseHelper<CollectionCurationResponse>.Error(
						"Request body cannot be null",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				if (request.CollectionId == null || request.CollectionId <= 0)
				{
					return ResponseHelper<CollectionCurationResponse>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				if (string.IsNullOrWhiteSpace(request.PinnedJson) &&
					string.IsNullOrWhiteSpace(request.ExcludeJson))
				{
					return ResponseHelper<CollectionCurationResponse>.Error(
						"PinnedJson or ExcludeJson must be provided",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				// 🔥 Call repository (SP: CollectionCuration_Save)
				var result = await _collectionRepository.UpsertCurationsAsync(request);

				if (result == null)
				{
					return ResponseHelper<CollectionCurationResponse>.Error(
						"Failed to save collection curations",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				// 🔥 Clear relevant caches
				_cache.Remove(COLLECTION_LIST_CACHE_KEY);
				_cache.Remove(CacheKeyBuilder.CollectionById(request.CollectionId.Value));
				_cache.Remove(CacheKeyBuilder.CollectionPreviewHotels(request.CollectionId.Value));

				return ResponseHelper<CollectionCurationResponse>.Success(
					"Collection curations saved successfully",
					result
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CollectionCurationResponse>.Error(
					"Error while saving collection curations",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<long>> CloneCollectionAsync(int sourceCollectionId)
		{
			try
			{
				if (sourceCollectionId <= 0)
				{
					return ResponseHelper<long>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var newId = await _collectionRepository.CloneCollectionAsync(sourceCollectionId);

				if (newId <= 0)
				{
					return ResponseHelper<long>.Error(
						"Failed to clone collection",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				// 🔥 Clear collection list cache
				_cache.Remove(COLLECTION_LIST_CACHE_KEY);
				_cache.Remove(CacheKeyBuilder.CollectionPreviewHotels((int)newId));

				return ResponseHelper<long>.Success(
					"Collection cloned successfully",
					newId
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<long>.Error(
					"Error while cloning collection",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<long>> DeleteCollectionAsync(int collectionId)
		{
			try
			{
				if (collectionId <= 0)
				{
					return ResponseHelper<long>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				// Call repository SP: Collection_Delete
				var deletedId = await _collectionRepository.DeleteCollectionAsync(collectionId);

				if (deletedId <= 0)
				{
					return ResponseHelper<long>.Error(
						"Collection not found or could not be deleted",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				// 🔥 Clear caches after deletion
				_cache.Remove(COLLECTION_LIST_CACHE_KEY);
				_cache.Remove(CacheKeyBuilder.CollectionById(collectionId));
				_cache.Remove(CacheKeyBuilder.CollectionPreviewHotels(collectionId));

				return ResponseHelper<long>.Success(
					"Collection deleted successfully",
					deletedId
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<long>.Error(
					"Error while deleting collection",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<List<CollectionPreviewHotelsResponse>>> GetCollectionPreviewHotelsAsync(int collectionId)
		{
			try
			{
				if (collectionId <= 0)
				{
					return ResponseHelper<List<CollectionPreviewHotelsResponse>>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var cacheKey = CacheKeyBuilder.CollectionPreviewHotels(collectionId);

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetCollectionPreviewHotelsAsync(collectionId),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(10)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<List<CollectionPreviewHotelsResponse>>.Error(
						"No hotels found for the given collection",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<List<CollectionPreviewHotelsResponse>>.Success(
					"Preview hotels fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<List<CollectionPreviewHotelsResponse>>.Error(
					"Error fetching preview hotels",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
