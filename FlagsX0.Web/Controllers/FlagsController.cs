using FlagsX0.Web.Business.UseCases.Flags;
using FlagsX0.Web.DTOs;
using FlagsX0.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace FlagsX0.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FlagsController(
        AddFlagUseCase addFlagUseCase,
        GetFlagsUseCase getFlagsUseCase,
        GetSingleFlagUseCase getSingleFlagUseCase,
        UpdateFlagUseCase updateFlagUseCase,
        DeleteFlagUseCase deleteFlagUseCase
    ) : Controller
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
            var singleFlag = await getSingleFlagUseCase.Execute(flagName);

            return View("SingleFlag", new SingleFlagViewModel()
            {
                Flag = singleFlag.Value,
                Message = message,
            });
        }

        [HttpPost("{flagName}")]
        public async Task<IActionResult> Update(FlagDTO flag)
        {
            if (flag.Name == null || flag.Name == "")
            {
                return View("SingleFlag", new SingleFlagViewModel()
                {
                    Flag = flag,
                    Message = "You must provide a valid name!",
                });
            }

            Result<FlagDTO> singleFlag = await updateFlagUseCase.Execute(flag);

            return View("SingleFlag", new SingleFlagViewModel()
            {
                Flag = singleFlag.Value,
                Message = singleFlag.Success ? "Updated Correctly" : singleFlag.Errors.First().Message,
            });
        }

        [HttpGet("delete/{flagName}")]
        public async Task<IActionResult> Delete(string flagName)
        {
            Result<bool> isDeleted = await deleteFlagUseCase.Execute(flagName);

            if (isDeleted.Success)
            {
                return RedirectToAction("");
            }

            return RedirectToAction("GetSingle", new
            {
                flagname = flagName,
                message = "Updated Correctly!"
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
