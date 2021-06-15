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
    public class NoblemenController : ControllerBase
    {
        private readonly ILogger<NoblemenController> _logger;
        private readonly KingdomContext _context;

        public NoblemenController(ILogger<NoblemenController> logger, KingdomContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("{noblemanId}")]
        public async Task<IActionResult> GetNoblemanById([FromRoute] int noblemanId)
        {
            var nobleman = await _context.Noblemen
                .Where(nobleman => nobleman.NoblemanId.Equals(noblemanId))
                .AsNoTracking()
                .FirstAsync();
            var response = new ResponseObject<Nobleman>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Nobleman>
                {
                    Results = new List<Nobleman> { nobleman }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpGet]
        [Route("{noblemanId}/responsibilities")]
        public async Task<IActionResult> GetNoblemenResponsibilities([FromRoute] int noblemanId, [FromQuery] PaginationQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            var responsibilities = _context.Responsibilities
                .Include(responsibility => responsibility.Noblemen.Where(nobleman => nobleman.NoblemanId.Equals(noblemanId)));
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

        [HttpPut]
        [Route("{noblemanId}")]
        public async Task<IActionResult> PutNobleman([FromRoute] int noblemanId, [FromBody] Nobleman nobleman)
        {
            nobleman.NoblemanId = noblemanId;
            _context.Noblemen.Add(nobleman);
            await _context.SaveChangesAsync();
            return new OkObjectResult(nobleman);
        }

        [HttpDelete]
        [Route("{noblemanId}")]
        public async Task<IActionResult> DeleteNobleman([FromRoute] int noblemanId)
        {
            var nobleman = new Nobleman
            {
                NoblemanId = noblemanId
            };
            _context.Noblemen.Remove(nobleman);
            await _context.SaveChangesAsync();
            return new OkObjectResult(nobleman);
        }
    }
}
