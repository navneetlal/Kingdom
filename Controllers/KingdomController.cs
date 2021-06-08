using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAll([FromQuery]GetAllKingdomQuery query)
        {   
            if(query.perPage > 100)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge);
            }
            try
            {
                var kingdom = _context.Kingdoms;
                var totalCount = kingdom.Count();
                var result = kingdom
                    .Skip((query.page - 1) * query.perPage)
                    .Take(query.perPage)
                    .AsNoTracking()
                    .ToList();
                var response = new ResponseObject<Kingdom>
                { 
                    Status = true,
                    Message = "Success",
                    Response = new Response<Kingdom>
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
        public IActionResult GetById(int? kingdomId)
        {
            var kingdom = _context.Kingdoms
                .Where(kingdom => kingdom.KingdomId.Equals(kingdomId))
                .Include(kingdom => kingdom.Clans)
                .Include(kingdom => kingdom.Noblemen)
                .AsNoTracking()
                .First();
            var response = new ResponseObject<Kingdom>
            { 
                Status = true,
                Message = "Success",
                Response = new Response<Kingdom>
                {
                    Results = new List<Kingdom>{ kingdom }
                }
            };
            return new OkObjectResult(response);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPut]
        public IActionResult Put()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }

    [BindProperties]
    public class GetAllKingdomQuery
    {
        public UInt16 page { get; set; } = 1;
        public UInt16 perPage { get; set; } = 10;
    }
}
