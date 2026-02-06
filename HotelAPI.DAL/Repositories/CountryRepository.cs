using Dapper;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Country;
using Microsoft.Data.SqlClient;
using System.Data;

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
		public async Task<CountryByUrlResponse?> GetCountryByUrlAsync_V2(string urlName,string? alphabet)
		{
			var parameters = new[] {
			new SqlParameter("@UrlName", urlName),
			new SqlParameter("@Alphabet", alphabet ?? (object)DBNull.Value)
		};

			var ds = await _sqlHelper.ExecuteDataSetAsync("Country_GetByUrl_v2",parameters);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return null;

			var countryRow = ds.Tables[0].Rows[0];

			var response = new CountryByUrlResponse
			{
				CountryID = Convert.ToInt32(countryRow["CountryID"]),
				CountryContent = countryRow["CountryContent"]?.ToString()
			};

			// Country Data (City + Region)
			response.CountryData = ds.Tables[1]
				.AsEnumerable()
				.Select(r => new CountryDataResponse
				{
					Id = r.Field<long>("Id"),
					ItemName = r.Field<string>("ItemName")!,
					UrlName = r.Field<string>("UrlName")!,
					Type = r.Field<string>("Type")!
				}).ToList();

			// Hotel Data
			response.HotelData = ds.Tables[2]
	           .AsEnumerable()
	           .Select(r => new HotelDataResponse
	           {
	           	Id = r.Field<long>("Id"),
	           	ItemName = r.Field<string>("ItemName")!,
	           	UrlName = r.Field<string>("UrlName")!,
	           	HotelCount = Convert.ToInt32(r["HotelCount"]), // ✅ FIX
	           	Type = r.Field<string>("Type")!
	           })
	           .ToList();
			return response;
		}
	}
}
