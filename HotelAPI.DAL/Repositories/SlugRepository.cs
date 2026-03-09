using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Slug;

namespace HotelAPI.DAL.Repositories
{
	public class SlugRepository(ISqlHelper _sqlHelper) : ISlugRepository
	{
		#region ResolveSlugAsync
		public async Task<GeoNodeResolveResponse?> ResolveSlugAsync(string slug)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@Slug", slug);

			var result = await _sqlHelper.QueryFirstOrDefaultAsync<GeoNodeResolveResponse>(
				StoredProcedure.GeoNodeResolveSlug,
				parameters
			);

			return result;
		}
		#endregion
	}
}
