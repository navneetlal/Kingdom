using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using KingdomApi.Services;

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
        public async Task<IActionResult> Login([FromBody] NoblemanCredential noblemanCredential)
        {
            var nobleman = await _context.Noblemen.Where(nobleman =>
                nobleman.Username.Equals(noblemanCredential.UsernameOrEmail) ||
                nobleman.EmailAddress.Equals(noblemanCredential.UsernameOrEmail)
            )
            .Include(nobleman => nobleman.Kingdom)
            .Include(nobleman => nobleman.Responsibilities)
            .AsNoTracking()
            .FirstAsync();

            if (PasswordHashManager.VerifyPassword(noblemanCredential.Password, nobleman.Password))
            {
                var token = JwtAuthManager.GenerateToken(nobleman);
                return new OkObjectResult(new { token });
            }
            else
            {
                return Ok();
            }
        }

        public class NoblemanCredential
        {
            public String UsernameOrEmail { get; set; }
            public String Password { get; set; }
        }
    }
}
