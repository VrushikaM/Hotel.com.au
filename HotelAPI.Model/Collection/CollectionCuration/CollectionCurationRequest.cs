namespace HotelAPI.Model.Collection.CollectionCuration
{
	public class CollectionCurationRequest
	{
		public int? CollectionId { get; set; }
		public string? PinnedJson { get; set; }
		public string? ExcludeJson { get; set; }
	}
}
