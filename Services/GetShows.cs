using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Extensions;
using Services.Models;

namespace Services
{
    public class GetShows :IGetShows
    {
        private readonly IElasticRepository _elasticRepository;

        public GetShows(IElasticRepository elasticRepository)
        {
            _elasticRepository = elasticRepository;
        }

        public async Task<List<Show>> GetSortedShowFromPage(int page)
        {
            var shows = await _elasticRepository.GetShowListSortedById(page);
            SortCastMembersByBirthDay(shows);
            return shows;
        }

        private static void SortCastMembersByBirthDay(IEnumerable<Show> shows)
        {
            foreach (var show in shows)
            {
                show.Cast=show.Cast.SortedOnBirthDay();
            }
        }
    }

    public interface IGetShows
    {
        Task<List<Show>> GetSortedShowFromPage(int page);
    }
}