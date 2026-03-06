using HotelAPI.BAL.Interfaces;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using HotelAPI.Model.Collection.CollectionCuration;
using HotelAPI.Model.Collection.CollectionRule;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{

	[ApiController]
	[Route("collections")]
	public class CollectionController(ICollectionService collectionService) : ControllerBase
	{
		/// <summary>
		/// Retrieves collections with optional filtering by status and geographic node.
		/// </summary>
		/// <param name="status">Optional collection status (e.g., Published, Draft).</param>
		/// <param name="geoNodeType">Optional geographic node type (Country, Region, City, District).</param>
		/// <param name="sourceId">Optional geographic node identifier.</param>
		/// <returns>Returns the filtered list of collections.</returns>

		[HttpGet]
		public async Task<IActionResult> GetCollectionList(
			[FromQuery] string? status,
			[FromQuery] string? geoNodeType,
			[FromQuery] int? sourceId,
			[FromQuery] int pageNumber = 1,
			[FromQuery] int pageSize = 10)
		{
			var result = await collectionService.GetCollectionListAsync(status, geoNodeType, sourceId, pageNumber, pageSize);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Creates a new collection.
		/// If `CollectionId` is provided in the request body, it will be ignored and a new collection is created.
		/// </summary>
		/// <param name="request">The collection details to create.</param>
		/// <returns>Returns the created collection with assigned CollectionId.</returns>

		[HttpPost]
		public async Task<IActionResult> UpsertCollection([FromBody] CollectionUpsertRequest request)
		{
			var result = await collectionService.UpsertCollectionAsync(request);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Retrieves basic collection items for the specified collection.
		/// </summary>
		/// <param name="id">Identifier of the collection.</param>
		/// <returns>Returns the list of basic collection items for the collection.</returns>

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCollection(int id)
		{
			var result = await collectionService.GetCollectionAsync(id);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Adds a new content item to the specified collection.
		/// </summary>
		/// <param name="id">Identifier of the collection.</param>
		/// <param name="request">Content details to add.</param>
		/// <returns>Returns the newly added content item.</returns>

		[HttpPost("{id}/content")]
		public async Task<IActionResult> UpsertContent(int id, [FromBody] CollectionContentRequest request)
		{
			request.CollectionId = id;
			var result = await collectionService.UpsertContentAsync(request);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Saves a collection rules based on the provided request details.
		/// </summary>
		/// <param name="request">The collection rules information to save.</param>
		/// <returns>Returns the saved collection rules and operation status.</returns>

		[HttpPost("rules")]
		public async Task<IActionResult> UpsertRules([FromBody] CollectionRuleRequest request)
		{
			var result = await collectionService.UpsertRulesAsync(request);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Changes the status of a collection to Draft or Publish.
		/// </summary>
		/// <param name="id">Identifier of the collection.</param>
		/// <param name="action">Status action (Draft or Publish).</param>
		/// <returns>Returns the updated collection identifier.</returns>

		[HttpPost("{id}/status")]
		public async Task<IActionResult> ChangeStatus(int id, [FromQuery] string action)
		{
			var result = await collectionService.ChangeStatusAsync(id, action);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Saves pinned and excluded hotels for a collection.
		/// </summary>
		/// <param name="request">Pinned and excluded hotel details.</param>
		/// <returns>Returns CollectionId and created ExclusionIds.</returns>

		[HttpPost("curations")]
		public async Task<IActionResult> UpsertCurations([FromBody] CollectionCurationRequest request)
		{
			var result = await collectionService.UpsertCurationsAsync(request);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Clones an existing collection as Draft.
		/// </summary>
		/// <param name="id">Source CollectionId to clone.</param>
		/// <returns>Returns the newly created CollectionId.</returns>

		[HttpPost("{id}/clone")]
		public async Task<IActionResult> CloneCollection(int id)
		{
			var result = await collectionService.CloneCollectionAsync(id);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Deletes a collection along with all its dependent data (content, rules, curations, revisions, and URL registry entry).
		/// </summary>
		/// <param name="id">Identifier of the collection to delete.</param>
		/// <returns>Returns the deleted CollectionId and a success message.</returns>

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCollection(int id)
		{
			var result = await collectionService.DeleteCollectionAsync(id);
			return StatusCode(result.Code, result);
		}

		/// <summary>
		/// Retrieves the list of hotels for a collection along with their reason (matched rule, pinned, etc.)
		/// </summary>
		/// <param name="id">CollectionId to preview hotels for.</param>
		/// <returns>Returns hotel count and hotel list with reason.</returns>

		[HttpGet("{id}/preview")]
		public async Task<IActionResult> GetCollectionPreviewHotels(int id)
		{
			var result = await collectionService.GetCollectionPreviewHotelsAsync(id);
			return StatusCode(result.Code, result);
		}
	}
}
