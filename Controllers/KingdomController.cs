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
    public class KingdomController : ControllerBase
    {
        private readonly ILogger<KingdomController> _logger;
        private readonly KingdomContext _context;

        public KingdomController(ILogger<KingdomController> logger, KingdomContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllKingdomQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var kingdom = _context.Kingdoms;
                var totalCount = await kingdom.CountAsync();
                var result = await kingdom
                    .Skip((query.Page - 1) * query.PerPage)
                    .Take(query.PerPage)
                    .AsNoTracking()
                    .ToListAsync();
                var response = new ResponseObject<Kingdom>
                {
                    Status = true,
                    Message = "Success",
                    Response = new Response<Kingdom>
                    {
                        Page = query.Page,
                        PerPage = query.PerPage,
                        Total = (uint)totalCount,
                        Results = result
                    }
                };
                return new OkObjectResult(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("kingdomId")]
        public async Task<IActionResult> GetById([FromRoute] uint kingdomId)
        {
            var kingdom = await _context.Kingdoms
                .Where(kingdom => kingdom.KingdomId.Equals(kingdomId))
                .Include(kingdom => kingdom.Clans)
                .Include(kingdom => kingdom.Noblemen)
                .AsNoTracking()
                .FirstAsync();
            var response = new ResponseObject<Kingdom>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Kingdom>
                {
                    Results = new List<Kingdom> { kingdom }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Kingdom kingdom)
        {
            _context.Kingdoms.Add(kingdom);
            await _context.SaveChangesAsync();
            return new CreatedResult(kingdom.KingdomId.ToString(), kingdom);
        }

        [HttpPut]
        [Route("kingdomId")]
        public async Task<IActionResult> Put([FromRoute] uint kingdomId, [FromBody] Kingdom kingdom)
        {
            kingdom.KingdomId = kingdomId;
            _context.Kingdoms.Add(kingdom);
            await _context.SaveChangesAsync();
            return new OkObjectResult(kingdom);
        }

        [HttpDelete]
        [Route("kingdomId")]
        public async Task<IActionResult> Delete([FromRoute] uint kingdomId)
        {
            var kingdom = new Kingdom
            {
                KingdomId = kingdomId
            };
            _context.Kingdoms.Remove(kingdom);
            await _context.SaveChangesAsync();
            return new OkObjectResult(kingdom);
        }
    }

    [BindProperties]
    public class GetAllKingdomQuery
    {
        public ushort Page { get; set; } = 1;
        public ushort PerPage { get; set; } = 10;
    }
}
