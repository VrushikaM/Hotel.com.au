using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Cache;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.MasterDropdowns;

namespace HotelAPI.BAL.Services
{
	public class MasterDropdownService(IMasterDropdownRepository _masterDropdownRepository, ICacheService _cache) : IMasterDropdownService
	{
		private const string MASTER_DROPDOWNS_CACHE_KEY = "masterDropdowns";

		public async Task<ResponseResult<MasterDropdownsResponse>> GetMasterDropdownsAsync()
		{
			try
			{
				var result = await _cache.GetOrCreateAsync(
					cacheKey: MASTER_DROPDOWNS_CACHE_KEY,
					factory: () => _masterDropdownRepository.GetMasterDropdownsAsync(),
					expiration: TimeSpan.FromMinutes(15),
					slidingExpiration: TimeSpan.FromMinutes(10)
				);

				var data = result ?? new MasterDropdownsResponse();

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
