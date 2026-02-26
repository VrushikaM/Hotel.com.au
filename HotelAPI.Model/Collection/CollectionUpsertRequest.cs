namespace HotelAPI.Model.Collection
{
	public class CollectionUpsertRequest
	{
		public int? CollectionId { get; set; }
		public string CollectionJson { get; set; } = default!;
		public string ChangedBy { get; set; } = default!;
	}
}
