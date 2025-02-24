namespace Web.Src.Service
{
    public interface ILocationService
    {
        Task<(double Latitude, double Longitude, string Country, string City, string Query)> GetUserLocationAsync();
    }
}
