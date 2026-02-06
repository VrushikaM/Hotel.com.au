using HotelAPI.Model.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.DAL.Interfaces
{
	public interface IGlobalSearchRepository
	{
		Task<IEnumerable<GlobalSearchResponse>>SearchAsync(string searchText);
	}
}
