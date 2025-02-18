using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaJWP.helper;

namespace PruebaJWP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post( string username, string password)
        {
            if(password == "ITESRC")
            {
                JwtTokenGenerator jwtToken = new();
                return Ok(jwtToken.GetToken(username));
            }
            else
            {
                return Unauthorized();
            }
        }


    }
}
