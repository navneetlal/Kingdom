using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetResponsibilityById([FromRoute] uint responsibilityId)
        {
            var responsibility = await _context.Responsibilities
                .Where(responsibility => responsibility.ResponsibilityId.Equals(responsibilityId))
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

        [HttpPut]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> PutResponsibility([FromRoute] uint responsibilityId, [FromBody] Responsibility responsibility)
        {
            responsibility.ResponsibilityId = responsibilityId;
            _context.Responsibilities.Add(responsibility);
            await _context.SaveChangesAsync();
            return new OkObjectResult(responsibility);
        }

        [HttpDelete]
        [Route("{responsibilityId}")]
        public async Task<IActionResult> DeleteResponsibility([FromRoute] uint responsibilityId)
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
