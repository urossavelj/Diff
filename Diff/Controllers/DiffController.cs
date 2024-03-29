using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Diff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiffController : ControllerBase
    {
        private readonly ILogger<DiffController> _logger;
        private readonly IMemoryCache _memoryCache;

        public DiffController(ILogger<DiffController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Saves left side of diff into memory cache
        /// </summary>
        /// <param name="id">Diff id</param>
        /// <param name="entryDiff">Left diff</param>
        /// <returns></returns>
        [HttpPost("{id}/left")]
        public IActionResult Left(string id, string entryDiff)
        {
            if (!_memoryCache.TryGetValue(id, out Diff? diff))
            {
                _logger.LogInformation("Diff doesn't exist yet, saving to memory", entryDiff);
                diff = new Diff();
                diff.Left = entryDiff;
                _memoryCache.Set(id, diff);
            }
            else
            {
                diff.Left = entryDiff;
                _memoryCache.Set(id, diff);
                _logger.LogInformation("Diff exists, updated entry in memory", diff);
            }

            return Ok();
        }

        /// <summary>
        /// Saves right side of diff into memory cache
        /// </summary>
        /// <param name="id">Diff id</param>
        /// <param name="entryDiff">Right diff</param>
        /// <returns></returns>
        [HttpPost("{id}/right")]
        public IActionResult Right(string id, string entryDiff)
        {
            if (!_memoryCache.TryGetValue(id, out Diff? diff))
            {
                _logger.LogInformation("Diff doesn't exist yet, saving to memory", entryDiff);
                diff = new Diff();
                diff.Right = entryDiff;
                _memoryCache.Set(id, diff);
            }
            else
            {
                diff.Right = entryDiff;
                _memoryCache.Set(id, diff);
                _logger.LogInformation("Diff exists, updated entry in memory", diff);
            }

            return Ok();
        }

        /// <summary>
        /// Gets diff result
        /// </summary>
        /// <param name="id">Diff id</param>
        /// <returns>Returns IActionResult</returns>
        [HttpGet]
        public IActionResult Get(string id)
        {
            _memoryCache.TryGetValue(id, out Diff? diff);

            if (diff != null)
            {
                if (diff.Left == null || diff.Right == null)
                    return BadRequest();

                if (diff.Left.SequenceEqual(diff.Right))
                    return Ok("Diffs are equal");
                if(diff.Left.Length != diff.Right.Length)
                    return Ok("Input not of equal size");
                else
                {
                    var differences = DiffLogic.FindDifference(diff);
                    return Ok(differences);
                }
            }
            else
                return NotFound();
        }
    }
}