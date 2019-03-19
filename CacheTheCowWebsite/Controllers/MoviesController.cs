using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CacheTheCowWebsite.Controllers
{
    [Route("api/movies")]
    public class MoviesController : Controller
    {
        IRepository repo;

        public MoviesController()
        {
            repo = GlobalObjects.MovieRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await repo.TryGetFirstMovie());

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(await repo.GetMovie(id));
    }
}
