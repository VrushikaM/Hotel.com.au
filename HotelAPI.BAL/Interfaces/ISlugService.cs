using HotelAPI.Common.Helper;
using HotelAPI.Model.Slug;

namespace HotelAPI.BAL.Interfaces
{
	public interface ISlugService
	{
		Task<ResponseResult<GeoNodeResolveResponse>> ResolveSlugAsync(string slug);
	}
}
