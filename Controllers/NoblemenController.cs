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
        public async Task<IActionResult> GetAll([FromRoute]UInt64 kingdomId , [FromQuery]GetAllNoblemanQuery query)
        {
            if(query.perPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var nobleman = _context.Noblemen.Where(nobleman => nobleman.KingdomId.Equals(kingdomId));
                var totalCount = await nobleman.CountAsync();
                var result = await nobleman
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
            catch (System.Exception)
            {   
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{noblemanId}")]
        public async Task<IActionResult> GetById([FromRoute]UInt64 noblemanId, [FromRoute]UInt64 kingdomId)
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
                    Results = new List<Nobleman>{ nobleman }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Nobleman nobleman)
        {
            _context.Noblemen.Add(nobleman);
            await _context.SaveChangesAsync();
            return new CreatedResult(nobleman.NoblemanId.ToString(), nobleman);
        }

        [HttpPut]
        [Route("{noblemanId}")]
        public async Task<IActionResult> Put([FromRoute]UInt64 noblemanId, [FromBody]Nobleman nobleman)
        {
            nobleman.NoblemanId = noblemanId;
            _context.Noblemen.Add(nobleman);
            await _context.SaveChangesAsync();
            return new OkObjectResult(nobleman);
        }

        [HttpDelete]
        [Route("{noblemanId}")]
        public async Task<IActionResult> Delete([FromRoute]UInt64 noblemanId)
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
        public UInt16 page { get; set; } = 1;
        public UInt16 perPage { get; set; } = 10;
    }
}
