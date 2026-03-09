using HotelAPI.Model.Slug;

namespace HotelAPI.DAL.Interfaces
{
	public interface ISlugRepository
	{
		Task<GeoNodeResolveResponse?> ResolveSlugAsync(string slug);
	}
}
