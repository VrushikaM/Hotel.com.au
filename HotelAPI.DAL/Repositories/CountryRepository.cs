using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Country;

namespace HotelAPI.DAL.Repositories
{
	public class CountryRepository(ISqlHelper _sqlHelper) : ICountryRepository
	{
		#region GetCountryListAsync
		public async Task<IEnumerable<CountryListResponse>> GetCountryListAsync(string? searchTerm)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@SearchTerm", searchTerm);

			return await _sqlHelper.QueryAsync<CountryListResponse>(
				StoredProcedure.GetCountryList,
				parameters
			);
		}
		#endregion

		#region GetCountryByUrlAsync
		public async Task<CountryByUrlResponse?> GetCountryByUrlAsync(string urlName, string? alphabet)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@UrlName", urlName);
			parameters.Add("@Alphabet", alphabet);

			return await _sqlHelper.QueryMultipleAsync(
				StoredProcedure.GetCountryByUrl,
				async multi =>
				{
					var country = await multi.ReadSingleOrDefaultAsync<CountryByUrlResponse>();

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
		#endregion
	}
}
