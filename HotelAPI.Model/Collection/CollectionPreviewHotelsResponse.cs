namespace HotelAPI.Model.Collection
{
	public class CollectionPreviewHotelsResponse
	{
		public long HotelId { get; set; }
		public string HotelName { get; set; } = string.Empty;
		public float ReviewScore { get; set; }
		public int Stars { get; set; }
		public string Reason { get; set; } = string.Empty;
	}
}
