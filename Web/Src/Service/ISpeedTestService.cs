namespace Web.Src.Service
{
    public interface ISpeedTestService
    {
        Task<double> FastDownloadSpeedAsync(TimeSpan duration);
    }
}
