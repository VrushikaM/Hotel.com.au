using HotelAPI.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/masterDropdown")]
	public class MasterDropdownController(IMasterDropdownService masterDropdownService) : ControllerBase
	{
		/// <summary>
		/// Retrieves the list of all master dropdowns.
		/// </summary>

		[HttpGet("list")]
		public async Task<IActionResult> GetMasterDropdowns()
		{
			var result = await masterDropdownService.GetMasterDropdownsAsync();
			return StatusCode(result.Code, result);
		}
	}
}
