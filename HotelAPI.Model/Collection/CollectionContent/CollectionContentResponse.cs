using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.Model.Collection.CollectionContent
{
	public class CollectionContentResponse
	{
		public int ContentId { get; set; }

		public int CollectionId { get; set; }

		public string? Header { get; set; }
		public string? MetaTitle { get; set; }
		public string? MetaDescription { get; set; }

		public string? IntroShortCopy { get; set; }
		public string? IntroLongCopy { get; set; }

		public string? HeroImageUrl { get; set; }
		public string? Badge { get; set; }

		public string? FAQsJson { get; set; }

		public int VersionNumber { get; set; }

		public bool IsDraft { get; set; }
		public bool IsPublished { get; set; }

		public DateTime CreatedDate { get; set; }
		public int CreatedBy { get; set; }

		public DateTime? UpdatedDate { get; set; }
		public int? UpdatedBy { get; set; }
	}
}
