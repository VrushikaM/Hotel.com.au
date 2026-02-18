namespace HotelAPI.Model.MasterDropdowns
{
	public class MasterDropdownsResponse
	{
		public List<GeoNodeListResponse> GeoNodes { get; set; } = [];
	}
	public class GeoNodeListResponse
	{
		public long GeoNodeId { get; set; }
		public string? Name { get; set; }
	}
}
