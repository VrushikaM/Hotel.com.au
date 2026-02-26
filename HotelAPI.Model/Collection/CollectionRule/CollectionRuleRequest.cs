namespace HotelAPI.Model.Collection.CollectionRule
{
	public class CollectionRuleRequest
	{
		public int CollectionId { get; set; }
		public string? RulesJson { get; set; }
	}
	//public class RulesRequest
	//{
	//	public int? RuleId { get; set; }
	//	public string Field { get; set; } = string.Empty;
	//	public string Operator { get; set; } = string.Empty;
	//	public string Value { get; set; } = string.Empty;
	//	public string? LogicalGroup { get; set; }
	//}
}
