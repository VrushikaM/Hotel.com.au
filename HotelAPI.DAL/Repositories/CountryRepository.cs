using Dapper;
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

		public async Task<CountryByUrlNameResponse?> GetCountryByUrlNameAsync(string urlName)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@UrlName", urlName);

			return await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.GetCountryByUrlName,
				async multi =>
				{
					var country = await multi.ReadFirstOrDefaultAsync<CountryByUrlNameResponse>();
					if (country == null)
						return null;

					var countryData = (await multi.ReadAsync<CountryDataResponse>()).ToList();
					country.CountryData = countryData;

					return country;
				},
				parameters
			);
		}
	}
}
