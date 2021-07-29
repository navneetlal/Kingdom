using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using KingdomApi.Models;

namespace KingdomApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoblesController : ControllerBase
    {
        private readonly ILogger<NoblesController> _logger;
        private readonly KingdomContext _context;

        public NoblesController(ILogger<NoblesController> logger, KingdomContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("{nobleId}")]
        public async Task<IActionResult> GetNobleById([FromRoute] int nobleId)
        {
            var noble = await _context.Nobles
                .Where(noble => noble.NobleId.Equals(nobleId))
                .AsNoTracking()
                .FirstAsync();
            var response = new ResponseObject<Noble>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Noble>
                {
                    Results = new List<Noble> { noble }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpGet]
        [Route("{nobleId}/responsibilities")]
        public async Task<IActionResult> GetNoblesResponsibilities([FromRoute] int nobleId, [FromQuery] PaginationQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            var responsibilities = _context.Responsibilities
                .Include(responsibility => responsibility.Nobles.Where(noble => noble.NobleId.Equals(nobleId)));
            var totalCount = await responsibilities.CountAsync();
            var result = await responsibilities
                .Skip((query.Page - 1) * query.PerPage)
                .Take(query.PerPage)
                .AsNoTracking()
                .ToListAsync();
            var response = new ResponseObject<Responsibility>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Responsibility>
                {
                    Results = result
                }
            };
            return new OkObjectResult(response);
        }

        [HttpPost]
        [Route("{nobleId}/add-responsibilities")]
        public async Task<IActionResult> AddResponsibilities([FromRoute] int nobleId, [FromBody] ICollection<Responsibility> responsibilities)
        {
            var noble = await _context.Nobles
                .Select(noble => new Noble
                {
                    NobleId = noble.NobleId,
                    Responsibilities = noble.Responsibilities
                        .Select(noble => new Responsibility
                        {
                            ResponsibilityId = noble.ResponsibilityId
                        })
                        .ToList()
                })
                .Where(noble => noble.NobleId.Equals(nobleId))
                .ToListAsync();
            if (noble.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            noble[0].Responsibilities
                .Concat(responsibilities)
                .GroupBy(responsibility => responsibility.ResponsibilityId)
                .Select(responsibility => responsibility.First());
            _context.Nobles.AddRange(noble);
            await _context.SaveChangesAsync();
            return new OkObjectResult(noble);
        }

        [HttpPost]
        [Route("{nobleId}/remove-responsibilities")]
        public async Task<IActionResult> RemoveResponsibilities([FromRoute] int nobleId, [FromBody] ICollection<Responsibility> responsibilities)
        {
            var noble = await _context.Nobles
                .Select(noble => new Noble
                {
                    NobleId = noble.NobleId,
                    Responsibilities = noble.Responsibilities
                        .Select(noble => new Responsibility
                        {
                            ResponsibilityId = noble.ResponsibilityId
                        })
                        .ToList()
                })
                .Where(noble => noble.NobleId.Equals(nobleId))
                .ToListAsync();
            if (noble.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            noble[0].Responsibilities = noble[0].Responsibilities
                .Where(r => responsibilities
                    .Any(s => !s.ResponsibilityId
                        .Equals(r.ResponsibilityId)))
                .ToList();
            _context.Nobles.AddRange(noble);
            await _context.SaveChangesAsync();
            return new OkObjectResult(noble);
        }

        [HttpPut]
        [Route("{nobleId}")]
        public async Task<IActionResult> PutNoble([FromRoute] int nobleId, [FromBody] Noble noble)
        {
            noble.NobleId = nobleId;
            _context.Nobles.Update(noble);
            await _context.SaveChangesAsync();
            return new OkObjectResult(noble);
        }

        [HttpDelete]
        [Route("{nobleId}")]
        public async Task<IActionResult> DeleteNoble([FromRoute] int nobleId)
        {
            var noble = new Noble
            {
                NobleId = nobleId
            };
            _context.Nobles.Remove(noble);
            await _context.SaveChangesAsync();
            return new OkObjectResult(noble);
        }
    }
}
