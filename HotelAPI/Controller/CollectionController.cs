using HotelAPI.BAL.Interfaces;
using HotelAPI.BAL.Services;
using HotelAPI.Model.Collection;
using HotelAPI.Model.Collection.CollectionContent;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controller
{

    [ApiController]
    [Route("api/collections")]
    public class CollectionController(ICollectionService collectionService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the list of collections with optional filtering by status, country, region, and city.
        /// Supports hierarchical filtering where city has highest priority,
        /// followed by region, then country. If no filters are provided,
        /// all collections are returned.
        /// </summary>
        /// <param name="status">Optional collection status (e.g., Published, Draft).</param>
        /// <param name="countryId">Optional country identifier for filtering country-level collections.</param>
        /// <param name="regionId">Optional region identifier for filtering region-level collections.</param>
        /// <param name="cityId">Optional city identifier for filtering city-level collections.</param>
        /// <returns>Returns the filtered list of collections.</returns>



        [HttpGet]
        public async Task<IActionResult> GetCollectionList([FromQuery] string? status, [FromQuery] int? countryId, [FromQuery] int? regionId, [FromQuery] int? cityId)
        {
            var result = await collectionService.GetCollectionListAsync(status, countryId, regionId, cityId);
            return StatusCode(result.Code, result);
        }

       

        [HttpPost]
        public async Task<IActionResult> CreateCollection([FromBody] CollectionUpsertRequest request)
        {
            request.CollectionId = null;

            var result = await collectionService.UpsertCollectionAsync(request);
            return StatusCode(result.Code, result);
        }


        [HttpPut("{collectionId}")]
        public async Task<IActionResult> UpdateCollection(int collectionId, [FromBody] CollectionUpsertRequest request)
        {
            request.CollectionId = collectionId;

            var result = await collectionService.UpsertCollectionAsync(request);

            return StatusCode(result.Code, result);
        }

        

    }
}
