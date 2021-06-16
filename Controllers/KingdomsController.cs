using System;
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
    public class KingdomsController : ControllerBase
    {
        private readonly ILogger<KingdomsController> _logger;
        private readonly KingdomContext _context;

        public KingdomsController(ILogger<KingdomsController> logger, KingdomContext context)
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{kingdomId}")]
        public async Task<IActionResult> GetKingdomById([FromRoute] int kingdomId)
        {
            var kingdom = await _context.Kingdoms
                .Where(kingdom => kingdom.KingdomId.Equals(kingdomId))
                .Include(kingdom => kingdom.Clans)
                .AsNoTracking()
                .ToListAsync();
            if(kingdom.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var response = new ResponseObject<Kingdom>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Kingdom>
                {
                    Results = kingdom
                }
            };
            return new OkObjectResult(response);
        }

        [HttpGet]
        [Route("{kingdomId}/clans")]
        public async Task<IActionResult> GetAllClan([FromRoute] int kingdomId, [FromQuery] PaginationQuery query)
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
        [Route("{kingdomId}/nobles")]
        public async Task<IActionResult> GetAllNoble([FromRoute] int kingdomId, [FromQuery] PaginationQuery query)
        {
            if (query.PerPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var noble = _context.Nobles.Where(noble => noble.KingdomId.Equals(kingdomId));
                var totalCount = await noble.CountAsync();
                var result = await noble
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get the list of responsibilities
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="kingdomId"></param>
        /// <param name="query"></param>
        /// <returns>Returns the list of responsibilities created inside the Kingdom</returns>
        [HttpGet]
        [Route("{kingdomId}/responsibilities")]
        public async Task<IActionResult> GetAllResponsibilities([FromRoute] int kingdomId, [FromQuery] PaginationQuery query)
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

        /// <summary>
        /// Creates a Kingdom.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Kingdom
        ///     {
        ///        "KingdomName": "My First Kingdom",
        ///        "Description": "This is my first kingdom"
        ///     }
        ///
        /// </remarks>
        /// <param name="kingdom"></param>
        /// <returns>A newly created Kingdom</returns>
        /// <response code="201">Returns the newly created Kingdom</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostKingdom([FromBody] Kingdom kingdom)
        {
            _context.Kingdoms.Add(kingdom);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetKingdomById), new { kingdomId = kingdom.KingdomId }, kingdom);
        }

        [HttpPost]
        [Route("{kingdomId}/clans")]
        public async Task<IActionResult> PostClan([FromRoute] int kingdomId, [FromBody] Clan clan)
        {
            clan.KingdomId = kingdomId;
            _context.Add(clan);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ClansController.GetClanById), new { clanId = clan.ClanId }, clan);
        }

        [HttpPost]
        [Route("{kingdomId}/nobles")]
        public async Task<IActionResult> PostNoble([FromRoute] int kingdomId, [FromBody] Noble noble)
        {
            noble.KingdomId = kingdomId;
            noble.NobleSecret = new NobleSecret
            {
                Username = noble.Username,
                EmailAddress = noble.EmailAddress,
                Password = PasswordHashManager.HashPassword(noble.Password)
            };
            _context.Nobles.Add(noble);
            await _context.SaveChangesAsync();
            noble.NobleSecret = null;
            return CreatedAtAction(nameof(ClansController.GetClanById), new { nobleId = noble.NobleId }, noble);
        }

        [HttpPost]
        [Route("{kingdomId}/responsibilities")]
        public async Task<IActionResult> PostResponsibility([FromRoute] int kingdomId, [FromBody] Responsibility responsibility)
        {
            responsibility.KingdomId = kingdomId;
            _context.Responsibilities.Add(responsibility);
            await _context.SaveChangesAsync();
            return new CreatedResult(responsibility.ResponsibilityId.ToString(), responsibility);
        }

        [HttpPut]
        [Route("{kingdomId}")]
        public async Task<IActionResult> PutKingdom([FromRoute] int kingdomId, [FromBody] Kingdom kingdom)
        {
            kingdom.KingdomId = kingdomId;
            _context.Kingdoms.Add(kingdom);
            await _context.SaveChangesAsync();
            return new OkObjectResult(kingdom);
        }

        [HttpDelete]
        [Route("{kingdomId}")]
        public async Task<IActionResult> DeleteKingdom([FromRoute] int kingdomId)
        {
            var kingdom = new Kingdom
            {
                KingdomId = kingdomId
            };
            try
            {
                _context.Kingdoms.Remove(kingdom);
                await _context.SaveChangesAsync();
                return new OkObjectResult(kingdom);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }

    [BindProperties]
    public class PaginationQuery
    {
        public ushort Page { get; set; } = 1;
        public ushort PerPage { get; set; } = 10;
    }
}
