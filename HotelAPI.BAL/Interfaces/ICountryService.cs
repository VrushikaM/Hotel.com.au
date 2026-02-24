using HotelAPI.Common.Helper;
using HotelAPI.Model.Country;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICountryService
	{
		Task<ResponseResult<IEnumerable<CountryListResponse>>> GetCountryListAsync(string? searchTerm);
		Task<ResponseResult<CountryByUrlResponse>> GetCountryByUrlAsync(string urlName, string? alphabet);
	}
}
