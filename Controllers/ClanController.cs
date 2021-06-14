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
    public class ClanController : ControllerBase
    {
        private readonly ILogger<ClanController> _logger;
        private readonly KingdomContext _context;

        public ClanController(ILogger<ClanController> logger, KingdomContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("{clanId}")]
        public async Task<IActionResult> GetClanById([FromRoute] uint clanId)
        {
            var clan = await _context.Clans
                .Where(clan => clan.ClanId.Equals(clanId))
                .Include(clan => clan.Noblemen)
                .AsNoTracking()
                .FirstAsync();
            var response = new ResponseObject<Clan>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Clan>
                {
                    Results = new List<Clan> { clan }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpGet]
        [Route("{clanId}/noblemen")]
        public async Task<IActionResult> GetClanNoblemen([FromRoute] uint clanId, [FromQuery] PaginationQuery query)
        {
            var noblemen = _context.Noblemen
                .Include(nobleman => nobleman.Clans.Where(clan => clan.ClanId.Equals(clanId)));
            var totalCount = await noblemen.CountAsync();
            var result = await noblemen
                .Skip((query.Page - 1) * query.PerPage)
                .Take(query.PerPage)
                .AsNoTracking()
                .ToListAsync();
            var response = new ResponseObject<Nobleman>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Nobleman>
                {
                    Page = query.Page,
                    PerPage = query.PerPage,
                    Total = (uint)totalCount,
                    Results = result
                }
            };
            return new OkObjectResult(response);
        }

        [HttpGet]
        [Route("{clanId}/responsibilities")]
        public async Task<IActionResult> GetClanResponsibilities([FromRoute] uint clanId, [FromQuery] PaginationQuery query)
        {
            var responsibilities = _context.Responsibilities
                .Include(responsibility => responsibility.Clans.Where(clan => clan.ClanId.Equals(clanId)));
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
        [Route("{clanId}")]
        public async Task<IActionResult> PutClan([FromRoute] uint clanId, [FromBody] Clan clan)
        {
            clan.ClanId = clanId;
            _context.Clans.Add(clan);
            await _context.SaveChangesAsync();
            return new OkObjectResult(clan);
        }

        [HttpDelete]
        [Route("{clanId}")]
        public async Task<IActionResult> DeleteClan([FromRoute] uint clanId)
        {
            var clan = new Clan
            {
                ClanId = clanId
            };
            _context.Clans.Remove(clan);
            await _context.SaveChangesAsync();
            return new OkObjectResult(clan);
        }
    }
}
