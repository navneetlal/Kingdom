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
    [Route("[controller]")]
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
        [Route("{responsibilityId}")]
        public async Task<IActionResult> GetResponsibilityById([FromRoute] Guid responsibilityId)
        {
            var responsibility = await _context.Responsibilities
                .Where(responsibility => responsibility.ResponsibilityId.Equals(responsibilityId))
                .AsNoTracking()
                .FirstAsync();
            var response = new ResponseObject<Responsibility>(HttpContext)
            {
                Status = StatusCodes.Status200OK,
                Title = "Success",
                Response = new Response<Responsibility>
                {
                    Results = new List<Responsibility> { responsibility }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpPut]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> PutResponsibility([FromRoute] Guid responsibilityId, [FromBody] Responsibility responsibility)
        {
            responsibility.ResponsibilityId = responsibilityId;
            _context.Responsibilities.Update(responsibility);
            await _context.SaveChangesAsync();
            return new OkObjectResult(responsibility);
        }

        [HttpDelete]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> DeleteResponsibility([FromRoute] Guid responsibilityId)
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
}
