using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("hotels")]
	public class HotelController(IHotelService hotelService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of hotel based on GeoNode Id.
		/// </summary>
		/// <param name="geoNodetype">Hotel name based on geoNodeType (country/region/district/city)</param>
		/// <param name="geoNodeId">GeoNode identifier (GeoNode Id)</param>
		/// <param name="searchTerm">Hotel name search keyword (Search)</param>
		/// <param name="maxCount">Hotel name based on maxCount</param>

		[HttpGet]
		public async Task<IActionResult> GetHotelsByGeoNode(string geoNodeType, int geoNodeId, string? searchTerm)
		{
			var result = await hotelService.GetHotelsByGeoNodeAsync(geoNodeType, geoNodeId, searchTerm);
			return StatusCode(result.Code, result);
		}
	}
}
