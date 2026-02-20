using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.Model.Collection.CollectionContent
{
	public class CollectionContentRequest
	{
		public int CollectionId { get; set; }
		public string Header { get; set; }
		public string MetaTitle { get; set; }
		public string MetaDescription { get; set; }
		public string IntroShortCopy { get; set; }
		public string IntroLongCopy { get; set; }
		public string HeroImageUrl { get; set; }
		public string Badge { get; set; }
		public string FAQsJson { get; set; }
	}
}
