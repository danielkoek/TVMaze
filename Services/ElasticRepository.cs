using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Services.Models;

namespace Services
{
    public class ElasticRepository : IElasticRepository
    {
        private readonly IElasticClient _client;

        public ElasticRepository(IElasticClient client)
        {
            _client = client;
        }

        public async Task InsertShows(IEnumerable<Show> shows)
        {
            var result = await _client.IndexManyAsync(shows);
            if (!result.IsValid) throw new ApplicationException(result.DebugInformation);
        }

        public async Task<List<Show>> GetShowListSortedById(int page = 1, int pageSize = 5)
        {
            if (page == 0) page = 1;
            var result = await _client.SearchAsync<Show>(s => s
                .Query(q => q
                    .MatchAll()
                ).Sort(ss=> ss.Ascending(p=> p.Id))
                .From((page - 1) * pageSize)
                .Size(pageSize)
            );
            return result.Documents.ToList();
        }

    }

    public interface IElasticRepository
    {
        Task InsertShows(IEnumerable<Show> shows);
        Task<List<Show>> GetShowListSortedById(int page = 1, int pageSize = 5);
    }
}