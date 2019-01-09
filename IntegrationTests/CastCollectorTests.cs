using System;
using System.Linq;
using System.Net.Http;
using Services.Collectors;
using Xunit;

namespace IntegrationTests
{
    public class CastCollectorTests
    {
        [Fact]
        public async void Collect1ShowsCast()
        {
            var client = new HttpClient {BaseAddress = new Uri("http://api.tvmaze.com/")};
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            ICastCollector castCollector= new CastCollector(client);
            var result= await castCollector.GetCastAsync(1);
            Assert.True(result.Any());
        }
        [Fact]
        public async void CollectAllShowsCast()
        {
            var client = new HttpClient {BaseAddress = new Uri("http://api.tvmaze.com/")};
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            IShowCollector showCollector= new ShowCollector(client);
            var shows = await showCollector.GetAllShowsAsync();
            foreach (var show in shows.Take(10)) // no need to collect them all
            {
                ICastCollector castCollector= new CastCollector(client);
                var result= await castCollector.GetCastAsync(show.Id);
                Assert.True(result.Any());
                
            }
           
        }
    }
}