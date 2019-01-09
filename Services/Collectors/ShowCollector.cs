using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Services.Collectors.Dto;

namespace Services.Collectors
{
    public class ShowCollector :IShowCollector
    {
        private readonly HttpClient _httpClient;

        public ShowCollector(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ShowDto>> GetAllShowsAsync()
        {
            var i=0;
            var allShows= new List<ShowDto>();
            while (true)
            {
                var dtoList = await GetShowListAsync(i);
                if (dtoList==null)
                {
                    break;
                }
                allShows.AddRange(dtoList);
                i++;
            }

            return allShows;
        }

        public async Task<IEnumerable<ShowDto>> GetShowListAsync(int page)
        {
            var response = await _httpClient.GetAsync("shows?page=" + page);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<ShowDto>>();
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            throw new CollectorException("Failed to collect shows for page" + page);
        }
    }
    public interface IShowCollector
    {
        Task<IEnumerable<ShowDto>> GetShowListAsync(int page);
        Task<IEnumerable<ShowDto>> GetAllShowsAsync();
    }
}
