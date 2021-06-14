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
    [Route("kingdom/{kingdomId}/[controller]")]
    public class NoblemenController : ControllerBase
    {
        private readonly ILogger<NoblemenController> _logger;
        private readonly KingdomContext _context;

        public NoblemenController(ILogger<NoblemenController> logger, KingdomContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] uint kingdomId, [FromQuery] GetAllNoblemanQuery query)
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
        [Route("{noblemanId}")]
        public async Task<IActionResult> GetById([FromRoute] uint noblemanId, [FromRoute] uint kingdomId)
        {
            var nobleman = await _context.Noblemen
                .Where(nobleman => nobleman.NoblemanId.Equals(noblemanId))
                .Where(nobleman => nobleman.KingdomId.Equals(kingdomId))
                .AsNoTracking()
                .FirstAsync();
            var response = new ResponseObject<Nobleman>
            {
                Status = true,
                Message = "Success",
                Response = new Response<Nobleman>
                {
                    Results = new List<Nobleman> { nobleman }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpGet]
        [Route("{noblemanId}/responsibilities")]
        public async Task<IActionResult> GetAllResponsibility([FromRoute] uint noblemanId, [FromRoute] uint kingdomId, [FromQuery] PaginationQuery query)
        {
            var responsibilities = _context.Responsibilities
                .Where(responsibility => responsibility.KingdomId.Equals(kingdomId))
                .Include(responsibility => responsibility.Noblemen.Where(nobleman => nobleman.NoblemanId.Equals(noblemanId)));
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
        public async Task<IActionResult> Post([FromRoute] uint kingdomId, [FromBody] Nobleman nobleman)
        {
            nobleman.Password = PasswordHashManager.HashPassword(nobleman.Password);
            nobleman.KingdomId = kingdomId;
            _context.Noblemen.Add(nobleman);
            await _context.SaveChangesAsync();
            return new CreatedResult(nobleman.NoblemanId.ToString(), nobleman);
        }

        [HttpPut]
        [Route("{noblemanId}")]
        public async Task<IActionResult> Put([FromRoute] uint noblemanId, [FromBody] Nobleman nobleman)
        {
            nobleman.NoblemanId = noblemanId;
            _context.Noblemen.Add(nobleman);
            await _context.SaveChangesAsync();
            return new OkObjectResult(nobleman);
        }

        [HttpDelete]
        [Route("{noblemanId}")]
        public async Task<IActionResult> Delete([FromRoute] uint noblemanId)
        {
            var nobleman = new Nobleman
            {
                NoblemanId = noblemanId
            };
            _context.Noblemen.Remove(nobleman);
            await _context.SaveChangesAsync();
            return new OkObjectResult(nobleman);
        }
    }

    [BindProperties]
    public class GetAllNoblemanQuery
    {
        public ushort Page { get; set; } = 1;
        public ushort PerPage { get; set; } = 10;
    }
}
