using HotelAPI.Common.Helper;
using HotelAPI.Model.Country;

namespace HotelAPI.BAL.Interfaces
{
	public interface ICountryService
	{
		Task<ResponseResult<IEnumerable<CountryListResponse>>> GetCountryListAsync();
		Task<ResponseResult<CountryByUrlNameResponse>> GetCountryByUrlNameAsync(string urlName, string? alphabet);
	}
}
