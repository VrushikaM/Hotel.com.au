using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.City;
using HotelAPI.Model.Search;

namespace HotelAPI.BAL.Services
{
	public class GlobalSearchService(IGlobalSearchRepository _globalSearchRepository) : IGlobalSearchService
	{

		public async Task<ResponseResult<IEnumerable<GlobalSearchResponse>>> SearchAsync(string searchText)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 2)
				{
					return ResponseHelper<IEnumerable<GlobalSearchResponse>>.Error(
						"Search text must contain at least 2 characters",
						statusCode: StatusCode.BAD_REQUEST
					);
				}

				var result = await _globalSearchRepository.SearchAsync(searchText.Trim());

				var data = result ?? Enumerable.Empty<GlobalSearchResponse>();
				
				return ResponseHelper<IEnumerable<GlobalSearchResponse>>.Success(
					"Search suggestions fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<GlobalSearchResponse>>.Error(
					"Failed to fetch search suggestions",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
