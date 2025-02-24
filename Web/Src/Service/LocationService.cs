using System.Text.Json;

namespace Web.Src.Service
{
    public class LocationService(HttpClient httpClient) : ILocationService
    {
        public virtual async Task<(double Latitude, double Longitude, string Country, string City, string Query)> GetUserLocationAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"http://ip-api.com/json/");

                if (!response.IsSuccessStatusCode)
                {
                    return (0, 0, "Unknown country", "Unknown city", "Unknown query");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseContent);

                var root = jsonDocument.RootElement;

                var latitude = root.TryGetProperty("lat", out var latElement)
                    ? latElement.GetDouble() : 0;
                var longitude = root.TryGetProperty("lon", out var lonElement)
                    ? lonElement.GetDouble() : 0;
                var country = root.TryGetProperty("country", out var countryElement)
                    ? countryElement.GetString() : "Unknown country";
                var city = root.TryGetProperty("city", out var cityElement)
                    ? cityElement.GetString() : "Unknown city";
                var query = root.TryGetProperty("query", out var queryElement)
                    ? queryElement.GetString() : "Unknown query";

                return (latitude, longitude, country, city, query)!;
            }
            catch
            {
                return (0, 0, "Unknown country", "Unknown city", "Unknown query");
            }
        }
    }
}
