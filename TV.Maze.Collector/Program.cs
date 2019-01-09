using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Services;
using Services.Collectors;
using Services.Extensions;

namespace TV.Maze.Collector
{
    public class Program
    {
        public static RetryPolicy<HttpResponseMessage> GetPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => (int) response.StatusCode == 429)
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                });
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var policy = GetPolicy();
            const string url = "http://localhost:9200/";
            services.AddElasticSearch(url,"shows");
            services.AddLogging(configure => configure.AddConsole());
            services.AddTransient<IElasticRepository, ElasticRepository>();
            services.AddSingleton<ICollector, Collector>();
            services.AddHttpClient<ICastCollector, CastCollector>(client =>
                {
                    client.BaseAddress = new Uri("http://api.tvmaze.com/");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                }
            ).AddPolicyHandler(policy);
            services.AddHttpClient<IShowCollector, ShowCollector>(client =>
                {
                    client.BaseAddress = new Uri("http://api.tvmaze.com/");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                }
            ).AddPolicyHandler(policy);
        }

        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var collector = serviceProvider.GetService<ICollector>();

            await collector.CollectAndSave();
        }
    }
}
