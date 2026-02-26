namespace HotelAPI.Model.Collection.CollectionCuration
{
	public class CurationByIdResponse
	{
		public List<PinnedHotelsByIdResponse>? PinnedHotels { get; set; }
		public List<ExcludedHotelsByIdResponse>? ExcludedHotels { get; set; }
	}
	public class PinnedHotelsByIdResponse
	{
		public long CollectionID { get; set; }
		public long HotelID { get; set; }
		public string? HotelName { get; set; }
		public int Position { get; set; }
		public string? PinType { get; set; }
	}

	public class ExcludedHotelsByIdResponse
	{
		public long ExclusionID { get; set; }
		public long CollectionID { get; set; }
		public long HotelID { get; set; }
		public string? HotelName { get; set; }
		public long? ChainID { get; set; }
		public string? Reason { get; set; }
	}
}
