using FlagsX0.Web.Business.UseCases.Flags;
using FlagsX0.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace FlagsX0.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FlagsController(AddFlagUseCase addFlagUseCase, GetFlagsUseCase getFlagsUseCase, GetSingleFlagUseCase getSingleFlag) : Controller
    {
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new FlagViewModel());
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddFlagToDatabase(FlagViewModel request)
        {

            Result<bool> isCreated = await addFlagUseCase.Execute(request.Name, request.IsEnabled);

            if (isCreated.Success)
            {
                return RedirectToAction("Index");
            }

            return View("Create", new FlagViewModel()
            {
                Error = isCreated.Errors.First().Message,
                IsEnabled = request.IsEnabled,
                Name = request.Name,
            });
        }

        [HttpGet("{flagName}")]
        public async Task<IActionResult> GetSingle(string flagName, string? message)
        {
            var singleFlag = await getSingleFlag.Execute(flagName);

            return View("SingleFlag", new SingleFlagViewModel()
            {
                Flag = singleFlag.Value,
                Message = message,
            });
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listFlags = await getFlagsUseCase.Execute();
            return View(new FlagIndexViewModel()
            {
                Flags = listFlags.Value,
            });
        }
    }
}
