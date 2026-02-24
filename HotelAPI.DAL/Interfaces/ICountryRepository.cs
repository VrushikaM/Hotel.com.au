using HotelAPI.Model.Country;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICountryRepository
	{
		Task<IEnumerable<CountryListResponse>> GetCountryListAsync(string? searchTerm);
		Task<CountryByUrlResponse?> GetCountryByUrlAsync(string urlName, string? alphabet);
	}
}
