using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Services.Collectors.Dto;

namespace Services.Collectors
{
    public class CastCollector :ICastCollector
    {
        private readonly HttpClient _httpClient;

        public CastCollector(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CastDto>> GetCastAsync(int showId)
        {
            var response = await _httpClient.GetAsync($"shows/{showId}/cast" );
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<CastDto>>();
            }
            throw new CollectorException("Failed to collect cast for show id" + showId);
        }
    }

    public interface ICastCollector
    {
        Task<IEnumerable<CastDto>> GetCastAsync(int showId);
    }
}
