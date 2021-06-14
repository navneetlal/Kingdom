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
    [Route("kingdom/{kingdomId}/[controller]")]
    public class ResponsibilitiesController : ControllerBase
    {
        private readonly ILogger<ResponsibilitiesController> _logger;
        private readonly KingdomContext _context;

        public ResponsibilitiesController(ILogger<ResponsibilitiesController> logger, KingdomContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] uint kingdomId, [FromQuery] GetAllResponsibilityQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var responsibility = _context.Responsibilities
                    .Where(responsibility => responsibility.KingdomId.Equals(kingdomId));
                var totalCount = await responsibility.CountAsync();
                var result = await responsibility
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
                        Page = query.Page,
                        PerPage = query.PerPage,
                        Total = (uint)totalCount,
                        Results = result
                    }
                };
                return new OkObjectResult(response);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> GetById([FromRoute] uint responsibilityId, [FromRoute] uint kingdomId)
        {
            var responsibility = await _context.Responsibilities
                .Where(responsibility => responsibility.ResponsibilityId.Equals(responsibilityId))
                .Where(responsibility => responsibility.KingdomId.Equals(kingdomId))
                .AsNoTracking()
                .FirstAsync();
            var response = new ResponseObject<Responsibility>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Responsibility>
                {
                    Results = new List<Responsibility> { responsibility }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] uint kingdomId, [FromBody] Responsibility responsibility)
        {
            responsibility.KingdomId = kingdomId;
            _context.Responsibilities.Add(responsibility);
            await _context.SaveChangesAsync();
            return new CreatedResult(responsibility.ResponsibilityId.ToString(), responsibility);
        }

        [HttpPut]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> Put([FromRoute] uint responsibilityId, [FromBody] Responsibility responsibility)
        {
            responsibility.ResponsibilityId = responsibilityId;
            _context.Responsibilities.Add(responsibility);
            await _context.SaveChangesAsync();
            return new OkObjectResult(responsibility);
        }

        [HttpDelete]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> Delete([FromRoute] uint responsibilityId)
        {
            var responsibility = new Responsibility
            {
                ResponsibilityId = responsibilityId
            };
            _context.Responsibilities.Remove(responsibility);
            await _context.SaveChangesAsync();
            return new OkObjectResult(responsibility);
        }
    }

    [BindProperties]
    public class GetAllResponsibilityQuery
    {
        public ushort Page { get; set; } = 1;
        public ushort PerPage { get; set; } = 10;
    }
}
