using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionRule;

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

		#endregion

		#region Upsert Collection

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
						"Failed to save collection",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

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
                if (collectionId <= 0)
                {
                    return ResponseHelper<CollectionContentResponse?>.Error(
                        "Valid CollectionId is required",
                        statusCode: StatusCode.UNPROCESSABLE_ENTITY
                    );
                }
                var cacheKey = $"{COLLECTION_CONTENT_CACHE_KEY}:{collectionId}";

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetAsync(collectionId),
					TimeSpan.FromMinutes(30),
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

		#endregion

		#region Get History

		public async Task<ResponseResult<IEnumerable<CollectionContentHistoryResponse>>> GetHistoryAsync(int collectionId)
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

                var cacheKey = $"{COLLECTION_HISTORY_CACHE_KEY}:{collectionId}";

				var data = await _cache.GetOrCreateAsync(
					cacheKey,
					() => _collectionRepository.GetHistoryAsync(collectionId),
					TimeSpan.FromMinutes(30),
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

		#endregion

		#region Save Rule

		public async Task<ResponseResult<int>> SaveRuleAsync(CollectionRuleRequest request)
		{
			try
			{
				var ruleId = await _collectionRepository.SaveRuleAsync(request);

				if (ruleId <= 0)
				{
					return ResponseHelper<int>.Error(
						"Failed to save rule",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				return ResponseHelper<int>.Success(
					request.RuleID == null
						? "Rule created successfully"
						: "Rule updated successfully",
					ruleId
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<int>.Error(
					"Error while saving rule",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		#endregion


		#region Get Rule By Id

		public async Task<ResponseResult<CollectionRuleResponse?>> GetRuleByIdAsync(int ruleId)
		{
			try
			{
				var data = await _collectionRepository.GetRuleByIdAsync(ruleId);

				if (data == null)
				{
					return ResponseHelper<CollectionRuleResponse?>.Error(
						"Rule not found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<CollectionRuleResponse?>.Success(
					"Rule fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CollectionRuleResponse?>.Error(
					"Error fetching rule",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		#endregion
	}
}