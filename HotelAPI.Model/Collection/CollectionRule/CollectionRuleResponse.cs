namespace HotelAPI.Model.Collection.CollectionRule
{
	public class CollectionRuleResponse
	{
		public List<Rules>? Rules { get; set; }
	}
	public class Rules
	{
		public int RuleId { get; set; }
		public int CollectionId { get; set; }
		public string Field { get; set; } = string.Empty;
		public string Operator { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
		public string? LogicalGroup { get; set; }
	}
}
