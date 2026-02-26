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
		private const string COLLECTION_CONTENT_CACHE_KEY = "collection:content";
		private const string COLLECTION_HISTORY_CACHE_KEY = "collection:history";

		public async Task<ResponseResult<IEnumerable<CollectionListResponse>>> GetCollectionListAsync(string? status, int? countryId, int? regionId, int? cityId)
		{
			try
			{
				var version = await _cache.GetOrCreateAsync(
					COLLECTION_LIST_CACHE_KEY,
					() => Task.FromResult(Guid.NewGuid().ToString()),
					TimeSpan.FromHours(1),
					TimeSpan.FromHours(1)
				);

				var cacheKey = $"{CacheKeyBuilder.CollectionList(status, countryId, regionId, cityId)}:{version}";

				var result = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetCollectionListAsync(status, countryId, regionId, cityId),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(10)
				);

				var data = result ?? Enumerable.Empty<CollectionListResponse>();

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

		public async Task<ResponseResult<CollectionContentResponse?>> GetContentAsync(int collectionId)
		{
			try
			{
				if (collectionId <= 0)
				{
					return ResponseHelper<CollectionContentResponse?>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var cacheKey = CacheKeyBuilder.CollectionContent(collectionId);

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetContentAsync(collectionId),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(10)
				);

				if (data == null)
				{
					return ResponseHelper<CollectionContentResponse?>.Error(
						"Collection content not found",
						statusCode: StatusCode.NOT_FOUND
					);
				}


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

		public async Task<ResponseResult<IEnumerable<CollectionContentHistoryResponse>>> GetContentHistoryAsync(int collectionId)
		{
			try
			{
				if (collectionId <= 0)
				{
					return ResponseHelper<IEnumerable<CollectionContentHistoryResponse>>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var cacheKey = CacheKeyBuilder.CollectionHistory(collectionId);

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetContentHistoryAsync(collectionId),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(10)
				);

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<CollectionContentHistoryResponse>>.Error(
						"No content history found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

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

		public async Task<ResponseResult<CollectionRuleResponse?>> GetRulesByIdAsync(int collectionId)
		{
			try
			{

				if (collectionId <= 0)
				{
					return ResponseHelper<CollectionRuleResponse?>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var cacheKey = CacheKeyBuilder.CollectionRule(collectionId);

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetRulesByIdAsync(collectionId),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(10)
				);

				if (data == null)
				{
					return ResponseHelper<CollectionRuleResponse?>.Error(
						"Rules not found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<CollectionRuleResponse?>.Success(
					"Rules fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CollectionRuleResponse?>.Error(
					"Error fetching rules",
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
				_cache.Remove($"{COLLECTION_CONTENT_CACHE_KEY}:{request.CollectionId}");
				_cache.Remove($"{COLLECTION_HISTORY_CACHE_KEY}:{request.CollectionId}");

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

		public async Task<ResponseResult<CurationByIdResponse?>> GetCurationsByIdAsync(int collectionId)
		{
			try
			{
				if (collectionId <= 0)
				{
					return ResponseHelper<CurationByIdResponse?>.Error(
						"Valid CollectionId is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var cacheKey = CacheKeyBuilder.CollectionCuration(collectionId);

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetCurationsByIdAsync(collectionId),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(10)
				);

				if (data == null)
				{
					return ResponseHelper<CurationByIdResponse?>.Error(
						"Collection curations not found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<CurationByIdResponse?>.Success(
					"Collection curations fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CurationByIdResponse?>.Error(
					"Error while fetching collection curations",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
