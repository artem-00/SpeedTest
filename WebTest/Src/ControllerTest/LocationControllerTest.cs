﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Src.Model;
using Web.Src.Model.Location;
using Web.Src.Service;
using Web.Src.Сontroller;

namespace WebTest.Src.ControllerTest

{
    [TestClass]
    public class LocationControllerTest
    {
        private Mock<ILocationService>? _mockLocationService;
        private Mock<IRedisCacheService>? _mockRedisCacheService;
        private LocationController? _locationController;

        [TestInitialize]
        public void Setup()
        {
            _mockLocationService = new Mock<ILocationService>();
            _mockRedisCacheService = new Mock<IRedisCacheService>();
            _locationController = new LocationController(_mockLocationService.Object, _mockRedisCacheService.Object);
        }

        [TestMethod]
        public async Task GetUserLocation_ReturnsOkWithLocation()
        {
            const double expectedLatitude = 40.7128;
            const double expectedLongitude = -74.0060;
            const string expectedCountry = "USA";
            const string expectedCity = "New York";
            const string expectedQuery = "New York, USA";

            _mockLocationService!.Setup(service => service.GetUserLocationAsync())
                .ReturnsAsync((expectedLatitude, expectedLongitude, expectedCountry, expectedCity, expectedQuery));

            var result = await _locationController!.GetUserLocation();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as UserLocationResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(expectedLatitude, response.Latitude);
            Assert.AreEqual(expectedLongitude, response.Longitude);
            Assert.AreEqual(expectedCountry, response.Country);
            Assert.AreEqual(expectedCity, response.City);
            Assert.AreEqual(expectedQuery, response.Query);
        }

        [TestMethod]
        public async Task GetUserLocation_ThrowsException_ReturnsInternalServerError()
        {
            _mockLocationService!.Setup(service => service.GetUserLocationAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _locationController!.GetUserLocation();

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);

            var errorMessage = objectResult.Value as string;
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual("Error retrieving location: Test exception", errorMessage);
        }

        [TestMethod]
        public async Task PostHostLocation_ValidIp_ReturnsOkWithLocation()
        {
            const string host = "8.8.8.8";
            var expectedLocation = new HostLocationResponse
            {
                Latitude = 37.3861,
                Longitude = -122.0838,
                Country = "United States",
                City = "Mountain View"
            };

            _mockLocationService!.Setup(service => service.GetLocationByIpAsync(host))
                .ReturnsAsync((expectedLocation.Latitude, expectedLocation.Longitude, expectedLocation.Country, expectedLocation.City));

            var result = await _locationController!.GetHostLocation(host);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as HostLocationResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(expectedLocation.Latitude, response.Latitude);
            Assert.AreEqual(expectedLocation.Longitude, response.Longitude);
            Assert.AreEqual(expectedLocation.Country, response.Country);
            Assert.AreEqual(expectedLocation.City, response.City);
        }

        [TestMethod]
        public async Task PostHostLocation_InvalidIp_ReturnsInternalServerError()
        {
            const string host = "invalid_ip";
            _mockLocationService!.Setup(service => service.GetLocationByIpAsync(host))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _locationController!.GetHostLocation(host);

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Error retrieving location: Test exception", objectResult.Value);
        }

        [TestMethod]
        public async Task GetClosestServer_ServerFound_ReturnsOkWithServer()
        {
            var closestServer = new Server
            {
                Country = "USA",
                City = "New York",
                Host = "server1.com",
                Provider = "Provider A",
                Latitude = 40.7128,
                Longitude = -74.0060
            };

            _mockLocationService!.Setup(service => service.GetClosestServerAsync())
                .ReturnsAsync(closestServer);

            var result = await _locationController!.GetClosestServer();

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);

            var response = objectResult.Value as ServerResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual("Closest server found", response.Message);

            Assert.AreEqual(closestServer.Country, response.Server!.Country);
            Assert.AreEqual(closestServer.City, response.Server.City);
            Assert.AreEqual(closestServer.Host, response.Server.Host);
            Assert.AreEqual(closestServer.Provider, response.Server.Provider);
            Assert.AreEqual(closestServer.Latitude, response.Server.Latitude);
            Assert.AreEqual(closestServer.Longitude, response.Server.Longitude);
        }

        [TestMethod]
        public async Task GetClosestServer_NoServerFound_ReturnsNotFound()
        {
            _mockLocationService!.Setup(service => service.GetClosestServerAsync())
                .ReturnsAsync((Server)null!);

            var result = await _locationController!.GetClosestServer();

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("No servers found", notFoundResult.Value);
        }

        [TestMethod]
        public async Task GetClosestServer_ExceptionThrown_ReturnsInternalServerError()
        {
            _mockLocationService!.Setup(service => service.GetClosestServerAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _locationController!.GetClosestServer();

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Server error: Test exception", objectResult.Value);
        }

        [TestMethod]
        public async Task GetBestServer_ServerFound_ReturnsOkWithServer()
        {
            var bestServer = new Server
            {
                Country = "USA",
                City = "New York",
                Host = "bestserver.com",
                Provider = "Provider A",
                Latitude = 40.7128,
                Longitude = -74.0060
            };

            _mockLocationService!.Setup(service => service.GetBestServerAsync())
                .ReturnsAsync(bestServer);

            var result = await _locationController!.GetBestServer();

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);

            var response = objectResult.Value as ServerResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual("Best server found", response.Message);

            Assert.AreEqual(bestServer.Country, response.Server!.Country);
            Assert.AreEqual(bestServer.City, response.Server.City);
            Assert.AreEqual(bestServer.Host, response.Server.Host);
            Assert.AreEqual(bestServer.Provider, response.Server.Provider);
            Assert.AreEqual(bestServer.Latitude, response.Server.Latitude);
            Assert.AreEqual(bestServer.Longitude, response.Server.Longitude);
        }

        [TestMethod]
        public async Task GetBestServer_NoServersFound_ReturnsNotFound()
        {
            _mockLocationService!.Setup(service => service.GetBestServerAsync())
                .ReturnsAsync((Server?)null);

            var result = await _locationController!.GetBestServer();

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("No servers found", notFoundResult.Value);
        }

        [TestMethod]
        public async Task GetBestServer_ExceptionThrown_ReturnsInternalServerError()
        {
            _mockLocationService!.Setup(service => service.GetBestServerAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _locationController!.GetBestServer();

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Server error: Test exception", objectResult.Value);
        }

        [TestMethod]
        public async Task GetServersOfCity_CacheHit_ReturnsServersFromCache()
        {
            const string city = "New York";
            var cachedServers = new List<Server>
            {
                new () { City = "New York", Host = "server1.com", Latitude = 40.7128, Longitude = -74.0060 },
                new () { City = "New York", Host = "server2.com", Latitude = 40.7138, Longitude = -74.0070 }
            };

            _mockRedisCacheService!.Setup(service => service.GetCachedValueAsync(It.IsAny<string>()))
                .ReturnsAsync(JsonSerializer.Serialize(cachedServers));

            var result = await _locationController!.GetServersOfCity(city);

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);

            var returnedServers = objectResult.Value as List<Server>;
            Assert.IsNotNull(returnedServers);
            Assert.AreEqual(2, returnedServers.Count);
            Assert.AreEqual("New York", returnedServers[0].City);

            _mockLocationService!.Verify(service => service.GetServersByCityAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetServersOfCity_CacheMiss_FetchesAndCachesServers()
        {
            const string city = "New York";
            var servers = new List<Server>
            {
                new () { City = "New York", Host = "server1.com", Latitude = 40.7128, Longitude = -74.0060 },
                new () { City = "New York", Host = "server2.com", Latitude = 40.7138, Longitude = -74.0070 }
            };

            _mockRedisCacheService!.Setup(service => service.GetCachedValueAsync(It.IsAny<string>()))
                .ReturnsAsync((string)null!);

            _mockRedisCacheService!.Setup(service =>
                    service.SetCachedValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()))
                .Returns(Task.CompletedTask);

            _mockRedisCacheService!.Setup(service => service.GetRedisInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<string, string> { { "used_memory_human", "1MB" } });

            _mockLocationService!.Setup(service => service.GetServersByCityAsync(city))
                .ReturnsAsync(servers);

            var result = await _locationController!.GetServersOfCity(city);

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult, "Result is not ObjectResult");
            Assert.AreEqual(200, objectResult.StatusCode);

            var returnedServers = objectResult.Value as List<Server>;
            Assert.IsNotNull(returnedServers, "Returned servers are null");
            Assert.AreEqual(2, returnedServers.Count);
            Assert.AreEqual("New York", returnedServers[0].City);

            _mockRedisCacheService.Verify(service =>
                service.SetCachedValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [TestMethod]
        public async Task GetServersOfCity_CacheError_ReturnsInternalServerError()
        {
            const string city = "New York";

            _mockRedisCacheService!.Setup(service => service.GetCachedValueAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Cache error"));

            var result = await _locationController!.GetServersOfCity(city);

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Server error: Cache error", objectResult.Value);
        }

        [TestMethod]
        public async Task GetServersOfCity_CacheMiss_ExceptionThrown_ReturnsInternalServerError()
        {
            const string city = "New York";

            _mockRedisCacheService!.Setup(service => service.GetCachedValueAsync(It.IsAny<string>()))
                .ReturnsAsync((string)null!);

            _mockLocationService!.Setup(service => service.GetServersByCityAsync(city))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _locationController!.GetServersOfCity(city);

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Server error: Test exception", objectResult.Value);
        }

    }
}
