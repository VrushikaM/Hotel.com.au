using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Country;

namespace HotelAPI.DAL.Repositories
{
	public class CountryRepository : ICountryRepository
	{
		private readonly ISqlHelper _sqlHelper;

		public CountryRepository(ISqlHelper sqlHelper)
		{
			_sqlHelper = sqlHelper;
		}

		public async Task<IEnumerable<CountryListResponse>> GetCountryListAsync()
		{
			return await _sqlHelper.QueryAsync<CountryListResponse>(
				StoredProcedure.GetCountryList
			);
		}
	}
}
