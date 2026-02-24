namespace HotelAPI.Model.MasterDropdowns
{
	public class MasterDropdownsResponse
	{
		public List<CountriesResponse> Countries { get; set; } = [];
	}
	public class CountriesResponse
	{
		public long CountryId { get; set; }
		public string? Name { get; set; }
	}
}
