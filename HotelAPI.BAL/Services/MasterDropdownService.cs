using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.MasterDropdowns;

namespace HotelAPI.BAL.Services
{
	public class MasterDropdownService(IMasterDropdownRepository _masterDropdownRepository, ICacheService _cache) : IMasterDropdownService
	{
		private const string MASTER_DRODOWNS_CACHE_KEY = "masterDropdowns";

		public async Task<ResponseResult<MasterDropdownsResponse>> GetMasterDropdownsAsync()
		{
			try
			{
				var data = await _cache.GetOrCreateAsync(
					cacheKey: MASTER_DRODOWNS_CACHE_KEY,
					factory: () => _masterDropdownRepository.GetMasterDropdownsAsync(),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(5)
				);

				if (data == null)
				{
					return ResponseHelper<MasterDropdownsResponse>.Error(
						"No dropdowns found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<MasterDropdownsResponse>.Success(
					"Dropdowns fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<MasterDropdownsResponse>.Error(
					"Failed to fetch dropdowns",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}