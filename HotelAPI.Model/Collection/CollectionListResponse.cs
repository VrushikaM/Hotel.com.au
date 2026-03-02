namespace HotelAPI.Model.Collection
{
	public class CollectionListResponse
	{
		public string? TotalRecords { get; set; }
		public List<CollectionData> Collections { get; set; } = new List<CollectionData>();
	}
	public class CollectionData
	{
		public long CollectionId { get; set; }
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
