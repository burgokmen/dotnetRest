using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WorkintechRestApiDemo.Business.Authentication;
using WorkintechRestApiDemo.Domain.Authenticaton;

namespace WorkintechRestApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IConfiguration config, IAuthenticationService authenticationService)
        {
            _config = config;
            _authenticationService = authenticationService;
        }
       

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(AuthenticationModel login)
        {
            IActionResult response = Unauthorized();
            var user =await _authenticationService.AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = _authenticationService.GenerateToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }
    }
}
