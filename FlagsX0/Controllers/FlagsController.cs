using FlagsX0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlagsX0.Controllers;

[Authorize]
[Route("[controller]")]
public class FlagsController : Controller
{
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new FlagViewModel());
    }
}