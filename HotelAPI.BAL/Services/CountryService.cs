using HotelAPI.BAL.Interfaces;
using HotelAPI.Common.Helper;
using HotelAPI.DAL.Interfaces;
using HotelAPI.Model.Country;

namespace HotelAPI.BAL.Services
{
	public class CountryService : ICountryService
	{
		private readonly ICountryRepository _countryRepository;

		public CountryService(ICountryRepository countryRepository)
		{
			_countryRepository = countryRepository;
		}

		public async Task<ResponseResult<IEnumerable<CountryListResponse>>> GetCountryListAsync()
		{
			try
			{
				var data = await _countryRepository.GetCountryListAsync();

				if (data == null || !data.Any())
				{
					return ResponseHelper<IEnumerable<CountryListResponse>>.Error(
						"No countries found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<IEnumerable<CountryListResponse>>.Success(
					"Country list fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<IEnumerable<CountryListResponse>>.Error(
					"Failed to fetch country list",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}

		public async Task<ResponseResult<CountryByUrlNameResponse>> GetCountryByUrlNameAsync(string urlName, string? aplhabet)
		{
			var data = await _countryRepository.GetCountryByUrlNameAsync(urlName, aplhabet);
			try
			{

				if (data == null)
				{
					return ResponseHelper<CountryByUrlNameResponse>.Error(
						"No country found",
						statusCode: StatusCode.NOT_FOUND
					);
				}

				return ResponseHelper<CountryByUrlNameResponse>.Success(
					"Country fetched successfully",
					data
				);
			}
			catch (Exception ex)
			{
				return ResponseHelper<CountryByUrlNameResponse>.Error(
					"Failed to fetch country",
					exception: ex,
					statusCode: StatusCode.INTERNAL_SERVER_ERROR
				);
			}
		}
	}
}
