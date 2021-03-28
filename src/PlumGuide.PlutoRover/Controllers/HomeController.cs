using Microsoft.AspNetCore.Mvc;
using PlumGuide.PlutoRover.Web.Interface;
using PlumGuide.PlutoRover.Web.Services;
using System;

namespace PlumGuide.PlutoRover.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly INavigateService _navigateService;

        public HomeController(INavigateService navigateService)
        {
            _navigateService = navigateService ?? throw new ArgumentNullException(nameof(navigateService));
        }

        [HttpGet]
        public IActionResult Index([FromQuery] NavigationCommand navigationCommand)
        {
            var result = _navigateService.Move(navigationCommand);
            if (result.Succeeded)
                return Ok(result.RoverPosition);

            return BadRequest(result.Errors);
        }
    }
}
