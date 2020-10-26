using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAdvertisements.AdvertApi.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new { Message = "Hello from home" });
        }
    }
}
