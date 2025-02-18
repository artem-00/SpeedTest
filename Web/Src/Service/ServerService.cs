﻿using Infrastructure;
using Newtonsoft.Json;
using Web.Src.Model;

namespace Web.Src.Service
{
    public class ServerService(IFileReader fileReader) : IServerService
    {
        private const string File = "..//Web//server.json";

        public async Task<List<Server>> GetServersAsync()
        {
            if (!await fileReader.ExistsAsync(File)) 
            {
                throw new FileNotFoundException("File server.json not found");
            }

            var jsonData = await fileReader.ReadAllTextAsync(File);

            try
            {
                var servers = JsonConvert.DeserializeObject<List<Server>>(jsonData);
                return servers ?? [];
            }
            catch (JsonSerializationException)
            {
                return [];
            }
        }

        public async Task AddServerAsync(Server server)
        {
            var servers = await GetServersAsync();
            var existingServer = servers.FirstOrDefault(s => s.Host.Equals(server.Host, StringComparison.OrdinalIgnoreCase));
            if (existingServer != null)
            {
                throw new Exception($"Сервер с хостом: {server.Host} уже существует");
            }

            servers.Add(server);

            var jsonData = JsonConvert.SerializeObject(servers);
            await System.IO.File.WriteAllTextAsync(File, jsonData);
        }

        public async Task UpdateServerAsync(List<Server> servers)
        {
            var jsonData = JsonConvert.SerializeObject(servers, Formatting.Indented);
            await fileReader.WriteAllTextAsync(File, jsonData);
        }

        public async Task DeleteServerAsync(string city, string? host = null)
        {
            var servers = await GetServersAsync();

            var cityServer = servers.Where(s =>
                s.City.Equals(city, StringComparison.OrdinalIgnoreCase)).ToList();

            if (cityServer.Count == 0)
            {
                throw new InvalidOperationException($"No server for city: {city}");
            }

            Server? serverToRemove;

            if (cityServer.Count > 1)
            {
                if (string.IsNullOrEmpty(host))
                {
                    throw new ArgumentException($"Multiple servers found for city: {city}. Please specify the host.");
                }

                serverToRemove = cityServer.FirstOrDefault(s =>
                    s.Host.Equals(host, StringComparison.OrdinalIgnoreCase));

                if (serverToRemove == null)
                {
                    throw new InvalidOperationException($"Server with host: {host} not found in city: {city}");
                }
            }
            else
            {
                serverToRemove = cityServer.First();
            }

            servers.Remove(serverToRemove);
            var jsonData = JsonConvert.SerializeObject(servers, Formatting.Indented);

            await fileReader.WriteAllTextAsync(File, jsonData);
        }

        public async Task DeleteAllServerAsync (string country)
        {
            var servers = await GetServersAsync();
            var updatedServer = servers.Where(s => 
                !s.Country.Equals(country, StringComparison.OrdinalIgnoreCase)).ToList();

            if (updatedServer.Count == servers.Count)
            {
                throw new InvalidOperationException($"Server for country: {country} not found");
            }

            var jsonData = JsonConvert.SerializeObject(updatedServer, Formatting.Indented);
            await fileReader.WriteAllTextAsync(File, jsonData);
        }
    }
}
