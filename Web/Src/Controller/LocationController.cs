using Microsoft.AspNetCore.Mvc;
using Web.Src.Service;
using Web.Src.Model.Location;

namespace Web.Src.Controller
{
    [ApiController]
    [Route("api/location")]
    public class LocationController(ILocationService locationService) : Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>
        /// Fetches the current user's location.
        /// </summary>
        /// <remarks>
        /// This method retrieves the geographical coordinates, country, and city of the current user.
        /// If the user's location cannot be determined, an error will be returned.
        /// </remarks>
        /// <response code="200">Successfully retrieved user location.</response>
        /// <response code="400">Invalid request parameters.</response>
        [HttpGet("my-location")]
        public async Task<IActionResult> GetUserLocation()
        {
            try
            {
                var location = await locationService.GetUserLocationAsync();
                var response = new UserLocationResponse
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Country = location.Country,
                    City = location.City,
                    Query = location.Query
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving location: {ex.Message}");
            }
        }
    }
}
