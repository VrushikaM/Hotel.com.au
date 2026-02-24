namespace HotelAPI.Model.Collection
{
	public class CollectionUpsertRequest
	{
		public int? CollectionId { get; set; }
		public string CollectionJson { get; set; } = default!;
		public string? RulesJson { get; set; }
		public string? PinnedJson { get; set; }
		public string? ExcludeJson { get; set; }
		public string ChangedBy { get; set; } = default!;
		public bool IsDebug { get; set; } = false;
	}
}
