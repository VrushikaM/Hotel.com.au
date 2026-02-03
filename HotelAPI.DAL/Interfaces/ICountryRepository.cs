using HotelAPI.Model.Country;

namespace HotelAPI.DAL.Interfaces
{
	public interface ICountryRepository
	{
		Task<IEnumerable<CountryListResponse>> GetCountryListAsync();
		Task<IEnumerable<ContentByCountryResponse>> GetContentByCountryAsync(long countryId);
	}
}
