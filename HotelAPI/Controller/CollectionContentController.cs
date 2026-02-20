using HotelAPI.BAL.Interfaces;
using HotelAPI.Model.Collection.CollectionContent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{
	[ApiController]
	[Route("api/collection/content")]
	public class CollectionContentController(
	ICollectionContentService _service) : ControllerBase
	{
		[HttpPost("save")]
		public async Task<IActionResult> Save(CollectionContentRequest request)
		{
			var result = await _service.SaveAsync(request);
			return StatusCode((int)result.Code, result);
		}

		[HttpGet("{collectionId}")]
		public async Task<IActionResult> Get(int collectionId)
		{
			var result = await _service.GetAsync(collectionId);
			return StatusCode((int)result.Code, result);
		}

		[HttpGet("history/{collectionId}")]
		public async Task<IActionResult> GetHistory(int collectionId)
		{
			var result = await _service.GetHistoryAsync(collectionId);
			return StatusCode((int)result.Code, result);
		}
	}
}
