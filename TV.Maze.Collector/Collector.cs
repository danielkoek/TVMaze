using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Services;
using Services.Collectors;
using Services.Collectors.Dto;
using Services.Extensions;
using Services.Models;

namespace TV.Maze.Collector
{
    class Collector: ICollector
    {
        private readonly ILogger _logger;
        private readonly IShowCollector _showCollector;
        private readonly ICastCollector _castCollector;
        private readonly IElasticRepository _elasticRepository;

        public Collector(ILogger<ICollector> logger, IShowCollector showCollector, ICastCollector castCollector, IElasticRepository elasticRepository)
        {
            _elasticRepository = elasticRepository;
            _logger = logger;
            _showCollector = showCollector;
            _castCollector = castCollector;
        }
        public async Task CollectAndSave()
        {
            _logger.LogInformation("Starting application");
            var showsDtoList = await _showCollector.GetAllShowsAsync();
            foreach (var batch in showsDtoList.Batch(100))
            {
                await FinishCreatingShowAndSaveAsync(batch);
            }
          
            _logger.LogInformation("All shows are done");
        }

        private async Task FinishCreatingShowAndSaveAsync(IEnumerable<ShowDto> batch)
        {
            var shows = await GetShowsAsync(batch);
            _logger.LogInformation("Inserting 100 shows");
            await _elasticRepository.InsertShows(shows);
            _logger.LogInformation("Done with 100 shows");
        }

        private async Task<List<Show>> GetShowsAsync(IEnumerable<ShowDto> showsDtoList)
        {
            var shows = new List<Show>();
            foreach (var s in showsDtoList)
            {
                shows.Add(new Show {Cast = await GetCastMembersAsync(s), Id = s.Id, Name = s.Name});
            }

            return shows;
        }

        private async Task<IEnumerable<CastMember>> GetCastMembersAsync(ShowDto showDto)
        {
            _logger.LogInformation("Collecting casts for show " + showDto.Id);
            var cast = await _castCollector.GetCastAsync(showDto.Id);
            var castMembers = cast.Select(c => new CastMember
                {Id = c.Person.Id, BirthDay = c.Person.Birthday, Name = c.Person.Name});
            return castMembers;
        }
    }

    public interface ICollector
    {
        Task CollectAndSave();
    }
}
