using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Message : ControllerBase
    {
        [HttpGet]
        public string GetMessage ()
        {
            return "Hola";
        }
    }
}
