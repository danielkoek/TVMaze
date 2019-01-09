using System;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Services.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(
            this IServiceCollection services, string url, string index)
        {
            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(index);


            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}