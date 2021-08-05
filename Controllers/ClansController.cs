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
    public class ClansController : ControllerBase
    {
        private readonly ILogger<ClansController> _logger;
        private readonly KingdomContext _context;

        public ClansController(ILogger<ClansController> logger, KingdomContext context)
        {
            _logger = logger;
            _context = context;
        }
      
        [HttpGet]
        [Route("{clanId}")]
        public async Task<IActionResult> GetClanById([FromRoute] Guid clanId)
        {
            var clan = await _context.Clans
                .Where(clan => clan.ClanId.Equals(clanId))
                .Include(clan => clan.Nobles)
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
        [Route("{clanId}/nobles")]
        public async Task<IActionResult> GetClanNobles([FromRoute] Guid clanId, [FromQuery] PaginationQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            var nobles = _context.Nobles
                .Include(noble => noble.Clans.Where(clan => clan.ClanId.Equals(clanId)));
            var totalCount = await nobles.CountAsync();
            var result = await nobles
                .Skip((query.Page - 1) * query.PerPage)
                .Take(query.PerPage)
                .AsNoTracking()
                .ToListAsync();
            var response = new ResponseObject<Noble>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Noble>
                {
                    Page = query.Page,
                    PerPage = query.PerPage,
                    Total = (uint)totalCount,
                    Results = result
                }
            };
            return new OkObjectResult(response);
        }

        [HttpPost]
        [Route("{clanId}/add-nobles")]
        public async Task<IActionResult> AddNobles([FromRoute] Guid clanId, [FromBody] ICollection<Noble> nobles)
        {
            var clan = await _context.Clans
                .Select(clan => new Clan
                {
                    ClanId = clan.ClanId,
                    Nobles = clan.Nobles
                        .Select(noble => new Noble
                        {
                            NobleId = noble.NobleId
                        })
                        .ToList()
                })
                .Where(clan => clan.ClanId.Equals(clanId))
                .ToListAsync();
            if (clan.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            clan[0].Nobles.Concat(nobles).GroupBy(noble => noble.NobleId).Select(noble => noble.First());
            _context.Clans.AddRange(clan);
            await _context.SaveChangesAsync();
            return new OkObjectResult(clan);
        }

        [HttpGet]
        [Route("{clanId}/responsibilities")]
        public async Task<IActionResult> GetClanResponsibilities([FromRoute] Guid clanId, [FromQuery] PaginationQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
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

        [HttpPost]
        [Route("{clanId}/add-responsibilities")]
        public async Task<IActionResult> AddResponsibilities([FromRoute] Guid clanId, [FromBody] ICollection<Responsibility> responsibilities)
        {
            var clan = await _context.Clans
                .Select(clan => new Clan
                {
                    ClanId = clan.ClanId,
                    Responsibilities = clan.Responsibilities
                        .Select(noble => new Responsibility
                        {
                            ResponsibilityId = noble.ResponsibilityId
                        })
                        .ToList()
                })
                .Where(clan => clan.ClanId.Equals(clanId))
                .ToListAsync();
            if (clan.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            clan[0].Responsibilities
                .Concat(responsibilities)
                .GroupBy(responsibility => responsibility.ResponsibilityId)
                .Select(responsibility => responsibility.First());
            _context.Clans.AddRange(clan);
            await _context.SaveChangesAsync();
            return new OkObjectResult(clan);
        }

        [HttpPut]
        [Route("{clanId}")]
        public async Task<IActionResult> PutClan([FromRoute] Guid clanId, [FromBody] Clan clan)
        {
            clan.ClanId = clanId;
            _context.Clans.Update(clan);
            await _context.SaveChangesAsync();
            return new OkObjectResult(clan);
        }

        [HttpDelete]
        [Route("{clanId}")]
        public async Task<IActionResult> DeleteClan([FromRoute] Guid clanId)
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
