namespace HotelAPI.Model.Collection
{
	public class CollectionByIdResponse
	{
		public long CollectionId { get; set; }
		public string? Name { get; set; }
		public string? Slug { get; set; }
		public long GeoNodeId { get; set; }
		public string? GeoName { get; set; }
		public string? Template { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public int? MaxHotels { get; set; }
		public string? Status { get; set; }
	}
}
