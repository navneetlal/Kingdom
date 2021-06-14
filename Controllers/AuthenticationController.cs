using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using KingdomApi.Models;

namespace KingdomApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly KingdomContext _context;

        public AuthController(ILogger<AuthController> logger, KingdomContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] NoblemanCredential noblemanCredential)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Logout([FromBody] NoblemanCredential noblemanCredential)
        {
            return Ok();
        }

        public class NoblemanCredential
        {

        }
    }
}
