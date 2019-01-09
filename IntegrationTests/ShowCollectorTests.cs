using System;
using System.Linq;
using System.Net.Http;
using Services.Collectors;
using Xunit;

namespace IntegrationTests
{
    public class ShowCollectorTests
    {
        [Fact]
        public async void Collect1Show()
        {
            var client = new HttpClient {BaseAddress = new Uri("http://api.tvmaze.com/")};
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            IShowCollector showCollector= new ShowCollector(client);
            var result= await showCollector.GetShowListAsync(0);
            Assert.True(result.Any());
        }
        [Fact]
        public async void CollectMultipleShows()
        {
            var client = new HttpClient {BaseAddress = new Uri("http://api.tvmaze.com/")};
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            IShowCollector showCollector= new ShowCollector(client);
            var result= await showCollector.GetAllShowsAsync();
            Assert.True(result.Any());
        }
    }
}
