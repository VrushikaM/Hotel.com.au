using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Slug;

namespace HotelAPI.BAL.Services
{
	public class SlugService(ISlugRepository _slugRepository) : ISlugService
	{
		public async Task<ResponseResult<GeoNodeResolveResponse>> ResolveSlugAsync(string slug)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(slug))
				{
					return ResponseHelper<GeoNodeResolveResponse>.Error(
						"Slug is required",
						statusCode: StatusCode.UNPROCESSABLE_ENTITY
					);
				}

				var data = await _slugRepository.ResolveSlugAsync(slug);

				if (data == null)
				{
					return ResponseHelper<GeoNodeResolveResponse>.Error(
						"Geo node not found for the provided slug",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<GeoNodeResolveResponse>.Success(
					"Geo node resolved successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<GeoNodeResolveResponse>.Error(
					"Error resolving slug",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
