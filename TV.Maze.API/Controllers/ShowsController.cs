using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace TV.Maze.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IGetShows _getShows;


        public ShowsController(IGetShows getShows)
        {
            _getShows = getShows;
        }
        // GET api/shows
        [HttpGet]
        public async Task<IActionResult> Get(int page)
        {
            var result = await _getShows.GetSortedShowFromPage(page);
            return Ok(result);
        }
    }
}
