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
        public async Task<IActionResult> GetAll([FromRoute] UInt32 kingdomId, [FromQuery] PaginationQuery query)
        {
            if (query.perPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var clan = _context.Clans.Where(clan => clan.KingdomId.Equals(kingdomId));
                var totalCount = await clan.CountAsync();
                var result = await clan
                    .Skip((query.page - 1) * query.perPage)
                    .Take(query.perPage)
                    .AsNoTracking()
                    .ToListAsync();
                var response = new ResponseObject<Clan>
                {
                    Status = true,
                    Message = "Success",
                    Response = new Response<Clan>
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
        [Route("{clanId}")]
        public async Task<IActionResult> GetById([FromRoute] UInt64 clanId, [FromRoute] UInt64 kingdomId)
        {
            var clan = await _context.Clans
                .Where(clan => clan.ClanId.Equals(clanId))
                .Where(clan => clan.KingdomId.Equals(kingdomId))
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
        public async Task<IActionResult> GetAllNoblemen([FromRoute] UInt64 clanId, [FromRoute] UInt64 kingdomId, [FromQuery] PaginationQuery query)
        {
            var noblemen = _context.Noblemen
                .Include(nobleman => nobleman.Clans.Where(clan => clan.ClanId.Equals(clanId)));
            var totalCount = await noblemen.CountAsync();
            var result = await noblemen
                .Skip((query.page - 1) * query.perPage)
                .Take(query.perPage)
                .AsNoTracking()
                .ToListAsync();
            var response = new ResponseObject<Nobleman>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Nobleman>
                {
                    Page = query.page,
                    PerPage = query.perPage,
                    Total = (UInt32)totalCount,
                    Results = result
                }
            };
            return new OkObjectResult(response);
        }

        [HttpGet]
        [Route("{clanId}/responsibilities")]
        public async Task<IActionResult> GetAllResponsibility([FromRoute] UInt64 clanId, [FromRoute] UInt64 kingdomId, [FromQuery] PaginationQuery query)
        {
            var responsibilities = _context.Responsibilities
                .Include(responsibility => responsibility.Clans.Where(clan => clan.ClanId.Equals(clanId)));
            var totalCount = await responsibilities.CountAsync();
            var result = await responsibilities
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
                    Results = result
                }
            };
            return new OkObjectResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] UInt32 kingdomId, [FromBody] Clan clan)
        {
            clan.KingdomId = kingdomId;
            _context.Add(clan);
            await _context.SaveChangesAsync();
            return new CreatedResult(clan.ClanId.ToString(), clan);
        }

        [HttpPut]
        [Route("{clanId}")]
        public async Task<IActionResult> Put([FromRoute] UInt32 clanId, [FromBody] Clan clan)
        {
            clan.ClanId = clanId;
            _context.Clans.Add(clan);
            await _context.SaveChangesAsync();
            return new OkObjectResult(clan);
        }

        [HttpDelete]
        [Route("{clanId}")]
        public async Task<IActionResult> Delete([FromRoute] UInt32 clanId)
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

    [BindProperties]
    public class PaginationQuery
    {
        public UInt16 page { get; set; } = 1;
        public UInt16 perPage { get; set; } = 10;
    }
}
