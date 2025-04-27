using FlagsX0.Web.Business.UseCases.Flags;
using FlagsX0.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlagsX0.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FlagsController : Controller
    {
        private readonly AddFlagUseCase _addFlagUseCase;
        private readonly GetFlagsUseCase _getFlagsUseCase;

        public FlagsController(AddFlagUseCase addFlagUseCase, GetFlagsUseCase getFlagsUseCase)
        {
            _addFlagUseCase = addFlagUseCase;
            _getFlagsUseCase = getFlagsUseCase;
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new FlagViewModel());
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddFlagToDatabase(FlagViewModel request)
        {

            bool isCreated = await _addFlagUseCase.Execute(request.Name, request.IsEnabled);

            return RedirectToAction("Index");
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listFlags = await _getFlagsUseCase.Execute();
            return View(new FlagIndexViewModel()
            {
                Flags = listFlags
            });
        }
    }
}
