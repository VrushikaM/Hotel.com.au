using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.Model.Collection.CollectionRule
{
	public class CollectionRuleRequest
	{
		public int? RuleID { get; set; }
		public int CollectionID { get; set; }
		public string Field { get; set; } = string.Empty;
		public string Operator { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
		public string? LogicalGroup { get; set; }
	}
}
