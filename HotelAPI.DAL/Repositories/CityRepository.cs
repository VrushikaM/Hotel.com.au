using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.City;

namespace HotelAPI.DAL.Repositories
{
	public class CityRepository(ISqlHelper _sqlHelper) : ICityRepository
	{
		#region GetCitiesByUrlRegistryAsync
		public async Task<IEnumerable<CitiesByUrlRegistryResponse>> GetCitiesByUrlRegistryAsync(int registryId)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@RegistryId", registryId);

			return await _sqlHelper.QueryAsync<CitiesByUrlRegistryResponse>(
				StoredProcedure.GetCitiesByUrlRegistry,
				parameters
			);
		}
		#endregion
	}
}
