using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using KingdomApi.Models;
using KingdomApi.Services;

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
        public async Task<IActionResult> GetAllKingdom([FromQuery] PaginationQuery query)
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
        [Route("{kingdomId}")]
        public async Task<IActionResult> GetKingdomById([FromRoute] uint kingdomId)
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

        [HttpGet]
        [Route("{kingdomId}/clan")]
        public async Task<IActionResult> GetAllClan([FromRoute] uint kingdomId, [FromQuery] PaginationQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var clan = _context.Clans.Where(clan => clan.KingdomId.Equals(kingdomId));
                var totalCount = await clan.CountAsync();
                var result = await clan
                    .Skip((query.Page - 1) * query.PerPage)
                    .Take(query.PerPage)
                    .AsNoTracking()
                    .ToListAsync();
                var response = new ResponseObject<Clan>
                {
                    Status = true,
                    Message = "Success",
                    Response = new Response<Clan>
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
        [Route("{kingdomId}/nobleman")]
        public async Task<IActionResult> GetAllNobleman([FromRoute] uint kingdomId, [FromQuery] PaginationQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var nobleman = _context.Noblemen.Where(nobleman => nobleman.KingdomId.Equals(kingdomId));
                var totalCount = await nobleman.CountAsync();
                var result = await nobleman
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
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{kingdomId}/responsibilities")]
        public async Task<IActionResult> GetAllResponsibilities([FromRoute] uint kingdomId, [FromQuery] PaginationQuery query)
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

        [HttpPost]
        public async Task<IActionResult> PostKingdom([FromBody] Kingdom kingdom)
        {
            _context.Kingdoms.Add(kingdom);
            await _context.SaveChangesAsync();
            return new CreatedResult(kingdom.KingdomId.ToString(), kingdom);
        }

        [HttpPost]
        [Route("{kingdomId}/clan")]
        public async Task<IActionResult> PostClan([FromRoute] uint kingdomId, [FromBody] Clan clan)
        {
            clan.KingdomId = kingdomId;
            _context.Add(clan);
            await _context.SaveChangesAsync();
            return new CreatedResult(clan.ClanId.ToString(), clan);
        }

        [HttpPost]
        [Route("{kingdomId}/nobleman")]
        public async Task<IActionResult> PostNobleman([FromRoute] uint kingdomId, [FromBody] Nobleman nobleman)
        {
            nobleman.Password = PasswordHashManager.HashPassword(nobleman.Password);
            nobleman.KingdomId = kingdomId;
            _context.Noblemen.Add(nobleman);
            await _context.SaveChangesAsync();
            return new CreatedResult(nobleman.NoblemanId.ToString(), nobleman);
        }

        [HttpPost]
        [Route("{kingdomId}/responsibilities")]
        public async Task<IActionResult> PostResponsibility([FromRoute] uint kingdomId, [FromBody] Responsibility responsibility)
        {
            responsibility.KingdomId = kingdomId;
            _context.Responsibilities.Add(responsibility);
            await _context.SaveChangesAsync();
            return new CreatedResult(responsibility.ResponsibilityId.ToString(), responsibility);
        }

        [HttpPut]
        [Route("kingdomId")]
        public async Task<IActionResult> PutKingdom([FromRoute] uint kingdomId, [FromBody] Kingdom kingdom)
        {
            kingdom.KingdomId = kingdomId;
            _context.Kingdoms.Add(kingdom);
            await _context.SaveChangesAsync();
            return new OkObjectResult(kingdom);
        }

        [HttpDelete]
        [Route("kingdomId")]
        public async Task<IActionResult> DeleteKingdom([FromRoute] uint kingdomId)
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
    public class PaginationQuery
    {
        public ushort Page { get; set; } = 1;
        public ushort PerPage { get; set; } = 10;
    }
}
