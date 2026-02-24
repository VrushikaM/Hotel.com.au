using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.MasterDropdowns;

namespace HotelAPI.DAL.Repositories
{
	public class MasterDropdownRepository(ISqlHelper _sqlHelper) : IMasterDropdownRepository
	{
		#region GetMasterDropdownsAsync
		public async Task<MasterDropdownsResponse> GetMasterDropdownsAsync()
		{
			return await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.MasterDropdowns,
				async multi =>
				{
					return new MasterDropdownsResponse
					{
						Countries = [.. await multi.ReadAsync<CountriesResponse>()]
					};
				}
			);
		}
		#endregion
	}
}
