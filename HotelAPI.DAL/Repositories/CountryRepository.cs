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

		public async Task<CountryByUrlResponse?> GetCountryByUrlAsync(string urlName, string? alphabet)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@UrlName", urlName);
			parameters.Add("@Alphabet", alphabet);

			return await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.GetCountryByUrl,
				async multi =>
				{
					var country = await multi.ReadFirstOrDefaultAsync<CountryByUrlResponse>();
					if (country == null)
						return null;

					var countryData = (await multi.ReadAsync<CountryDataResponse>()).ToList();
					var hotelData = (await multi.ReadAsync<HotelDataResponse>()).ToList();
					
					country.CountryData = countryData;
					country.HotelData = hotelData;

					return country;
				},
				parameters
			);
		}
	}
}
