using System;
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
        public async Task<IActionResult> GetAll([FromRoute] UInt32 kingdomId, [FromQuery] GetAllResponsibilityQuery query)
        {
            if (query.perPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var responsibility = _context.Responsibilities
                    .Where(responsibility => responsibility.KingdomId.Equals(kingdomId));
                var totalCount = await responsibility.CountAsync();
                var result = await responsibility
                    .Skip((query.page - 1) * query.perPage)
                    .Take(query.perPage)
                    .AsNoTracking()
                    .ToListAsync();
                var response = new ResponseObject<Responsibility>
                {
                    Status = true,
                    Message = "Success",
                    Response = new Response<Responsibility>
                    {
                        Page = query.page,
                        PerPage = query.perPage,
                        Total = (UInt32)totalCount,
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
        public async Task<IActionResult> GetById([FromRoute] UInt32 responsibilityId, [FromRoute] UInt32 kingdomId)
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
        public async Task<IActionResult> Post([FromBody] Responsibility responsibility)
        {
            _context.Responsibilities.Add(responsibility);
            await _context.SaveChangesAsync();
            return new CreatedResult(responsibility.ResponsibilityId.ToString(), responsibility);
        }

        [HttpPut]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> Put([FromRoute] UInt32 responsibilityId, [FromBody] Responsibility responsibility)
        {
            responsibility.ResponsibilityId = responsibilityId;
            _context.Responsibilities.Add(responsibility);
            await _context.SaveChangesAsync();
            return new OkObjectResult(responsibility);
        }

        [HttpDelete]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> Delete([FromRoute] UInt32 responsibilityId)
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
        public UInt16 page { get; set; } = 1;
        public UInt16 perPage { get; set; } = 10;
    }
}
