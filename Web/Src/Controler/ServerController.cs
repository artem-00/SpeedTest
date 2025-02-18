﻿using Microsoft.AspNetCore.Mvc;
using Web.Src.Service;
using Web.Src.Model;

namespace Web.Src.Сontroller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServerController(IServerService serverService) : Controller
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetServerList()
        {
            try
            {
                var servers = await serverService.GetServersAsync();
                return Ok(servers);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddServer([FromBody] Server server)
        {
            if (server == null || string.IsNullOrEmpty(server.Host))
            {
                return BadRequest("Введите корректные данные");
            }
            try
            {
                await serverService.AddServerAsync(server);
                return Ok($"Сервер успешно добавлен ({server.Host}).\nСтрана: {server.Country}\nГород: {server.City}\n");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateHost([FromBody] UpdateHostRequest? request)
        {
            if (request == null || string.IsNullOrEmpty(request.OldHost) || 
                string.IsNullOrEmpty(request.NewHost))
            {
                return BadRequest("Enter correct data");
            }
            try
            {
                var servers = await serverService.GetServersAsync();
                var server = servers.FirstOrDefault(s => 
                    s.Host.Equals(request.OldHost, StringComparison.OrdinalIgnoreCase));
                if (server == null)
                {
                    return NotFound($"Old host: {request.OldHost} not found");
                }
                var oldHost = server.Host;
                server.Host = request.NewHost;
                await serverService.UpdateServerAsync(servers);
                return Ok($"Old host {request.OldHost} update form " +
                          $"{oldHost} to {request.NewHost}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete("delete-server")]
        public async Task<IActionResult> DeleteServer([FromQuery] string city, [FromQuery] string? host = null)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("OldHost can't be null or empty");
            }

            try
            {
                await serverService.DeleteServerAsync(city, host);
                return Ok($"Server successfully deleted for city: {city} with host {host}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete("delete-all")]
        public async Task<IActionResult> DeleteAllServer([FromBody] string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return BadRequest("OldHost can't be null or empty");
            }
            try
            {
                await serverService.DeleteAllServerAsync(country);
                return Ok($"All servers for country: '{country}' successfully deleted");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
