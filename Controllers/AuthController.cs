using jwt_net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet("{user}/{password}")]
        public async Task<string> GetJWT(string user, string password)
        {
            return await _authService.GetJWT(user, password);
        }
    }
}
