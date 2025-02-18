using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PruebaJWP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]
    public class SaludosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var context = Request.HttpContext;

            if (User.Identity != null)
            {
              return Ok("Hola" + User.Identity.Name);
            }
            return Ok();
        }
    }
}
