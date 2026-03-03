using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionCuration;
using HotelAPI.Model.Collection.CollectionRule;

namespace HotelAPI.Model.Collection
{
	public class CollectionByIdResponse
	{
		public BasicCollectionResponse? BasicCollection { get; set; } = new();
		public CollectionContentResponse? CollectionContent { get; set; } = new();
		public CollectionContentHistoryResponse? CollectionContentHistory { get; set; } = new();
		public List<CollectionRuleResponse> CollectionRules { get; set; } = new();
		public List<CurationByIdResponse> CollectionCuration { get; set; } = new();
	}
	public class BasicCollectionResponse
	{
		public long CollectionId { get; set; }
		public string? Name { get; set; }
		public string? Slug { get; set; }
		public string? Template { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public int? MaxHotels { get; set; }
		public string? Status { get; set; }
		public long CountryId { get; set; }
		public string? CountryName { get; set; }
		public long RegionId { get; set; }
		public string? RegionName { get; set; }
		public long CityId { get; set; }
		public string? CityName { get; set; }
		public long DistrictId { get; set; }
		public string? DistrictName { get; set; }
	}
}
