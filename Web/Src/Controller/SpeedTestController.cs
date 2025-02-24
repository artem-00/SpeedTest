using Microsoft.AspNetCore.Mvc;
using Web.Src.Service;
using Web.Src.Model.SpeedTest;


namespace Web.Src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeedTestController(ISpeedTestService speedTestService) : Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>
        /// Performs a fast download speed test.
        /// </summary>
        /// <remarks>
        /// This method quickly measures the download speed within a 7-second timeout.
        /// It is designed for fast and approximate speed testing.
        /// </remarks>
        /// <response code="200">Successfully retrieved the fast download speed result in Mbps.</response>
        /// <response code="500">An unexpected server error occurred during the fast speed test.</response>
        [HttpGet("fast-test")]
        public async Task<IActionResult> GetFastDownloadSpeed()
        {
            try
            {
                var speed = await speedTestService.FastDownloadSpeedAsync(TimeSpan.FromSeconds(7));
                return Ok(new DownloadSpeedResponse
                {
                    Speed = speed
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
