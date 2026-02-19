namespace HotelAPI.Model.MasterDropdowns
{
	public class MasterDropdownsResponse
	{
		public List<UrlRegistryListResponse> UrlRegistries { get; set; } = [];
	}
	public class UrlRegistryListResponse
	{
		public long RegistryId { get; set; }
		public string? Slug { get; set; }
	}
}
