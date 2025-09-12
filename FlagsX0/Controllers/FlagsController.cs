using FlagsX0.Business.UseCases;
using FlagsX0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlagsX0.Controllers;

[Authorize]
[Route("[controller]")]
public class FlagsController(
    AddFlagUseCase addFlagUseCase,
    GetFlagUseCase getFlagUseCase) : Controller
{
    private readonly AddFlagUseCase _addFlagUseCase = addFlagUseCase;
    private readonly GetFlagUseCase _getFlagUseCase = getFlagUseCase;

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new FlagViewModel());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> AddFlagToDataBase(FlagViewModel request)
    {
        var isCreated = await _addFlagUseCase.Execute(request.Name, request.IsEnabled);

        if (isCreated.Success) return RedirectToAction("Index");

        return View("Create", new FlagViewModel
        {
            Name = request.Name,
            Error = isCreated.Errors.First().Message,
            IsEnabled = request.IsEnabled
        });
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var listFlags = await _getFlagUseCase.Execute();

        if (listFlags.Success)
            return View(new FlagIndexViewModel
            {
                Flags = listFlags.Value
            });

        return View(new FlagIndexViewModel
        {
            Flags = [],
            Error = listFlags.Errors.First().Message
        });
    }
}