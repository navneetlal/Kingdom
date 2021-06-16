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
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] NobleCredential nobleCredential)
        {
            var noble = await _context.Nobles.Where(noble =>
                noble.Username.Equals(nobleCredential.UsernameOrEmail) ||
                noble.EmailAddress.Equals(nobleCredential.UsernameOrEmail)
            )
            .Include(noble => noble.Kingdom)
            .Include(noble => noble.Responsibilities)
            .AsNoTracking()
            .FirstAsync();

            if (PasswordHashManager.VerifyPassword(nobleCredential.Password, noble.Password))
            {
                var token = JwtAuthManager.GenerateToken(noble);
                return new OkObjectResult(new { token });
            }
            else
            {
                return Ok();
            }
        }

        public class NobleCredential
        {
            public String UsernameOrEmail { get; set; }
            public String Password { get; set; }
        }
    }
}
