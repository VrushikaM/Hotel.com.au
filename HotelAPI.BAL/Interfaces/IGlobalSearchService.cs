using HotelAPI.Common.Helper;
using HotelAPI.Model.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.BAL.Interfaces
{
	public interface IGlobalSearchService
	{
		Task<ResponseResult<IEnumerable<GlobalSearchResponse>>>SearchAsync(string searchText);
	}
}
