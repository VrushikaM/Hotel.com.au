namespace HotelAPI.Model.Collection
{
	public class CollectionListResponse
	{
		public long CollectionID { get; set; }
		public string? Name { get; set; }
		public string? Slug { get; set; }
		public string? Type { get; set; }
		public string? Status { get; set; }
		public DateTime? PublishDate { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public int? MaxHotels { get; set; }
		public int HotelCount { get; set; }
	}
}
