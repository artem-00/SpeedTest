using System.Diagnostics;
using System.Drawing;

namespace Web.Src.Service
{
    public class SpeedTestService(
        IConfiguration configuration,
        Func<HttpClient> httpClientFactory) : ISpeedTestService
    {
        private const int Buffer = 8192;
        private const double Size = 1024;

        public async Task<double> FastDownloadSpeedAsync(TimeSpan duration)
        {
            var url = configuration["SpeedTest:DownloadUrl"]
                      ?? throw new InvalidOperationException("Download URL isn't configured");

            double totalDownloaded = 0;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using var cts = new CancellationTokenSource();
                cts.CancelAfter(duration);

                var downloadedBytes = await DownloadFileAsync(url, cts.Token);
                totalDownloaded += downloadedBytes;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Download operation was canceled due to timeout.");
            }
            catch (Exception ex)
            {
                throw new Exception("Speed ​​measurement error", ex);
            }
            finally
            {
                stopwatch.Stop();
            }

            if (stopwatch.Elapsed.TotalSeconds == 0 || totalDownloaded == 0)
            {
                return 0;
            }

            var speedInMbps = ((totalDownloaded / Size / Size) / stopwatch.Elapsed.TotalSeconds) * 8;
            return Math.Round(speedInMbps, 3);
        }

        private async Task<long> DownloadFileAsync(string url, CancellationToken cancellationToken)
        {
            long totalBytesRead = 0;

            using var httpClient = httpClientFactory();
            httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                                              "(KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            try
            {
                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error download. HTTP status: {response.StatusCode}");
                }

                await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var buffer = new byte[Buffer];

                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                {
                    totalBytesRead += bytesRead;
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Error downloading file", ex);
            }

            return totalBytesRead;
        }
    }
}
