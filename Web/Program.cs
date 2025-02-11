using Microsoft.OpenApi.Models;
using Infrastructure;
using Web.Src.Service;


namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Speed Test",
                    Version = "v1",
                    Description = "Api for testing download speed"
                });
            });

            builder.Services.AddScoped<Func<HttpClient>>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                return () => httpClientFactory.CreateClient();
            });

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IFileReader, FileReader>();
            builder.Services.AddScoped<IServerService, ServerService>();
            builder.Services.AddControllers();
            

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                    options.RoutePrefix = "swagger";
                });
            }

            app.MapControllers();
            app.Run();
        }
    }
}