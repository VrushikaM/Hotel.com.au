namespace HotelAPI.Model.Collection.CollectionCuration
{
	public class CollectionCurationResponse
	{
		public PinnedHotelsResponse? PinnedHotels { get; set; }
		public ExcludedHotelsResponse? ExcludedHotels { get; set; }
	}
	public class PinnedHotelsResponse
	{
		public long CollectionId { get; set; }
	}

	public class ExcludedHotelsResponse
	{
		public List<long>? ExclusionIds { get; set; }
	}
}
