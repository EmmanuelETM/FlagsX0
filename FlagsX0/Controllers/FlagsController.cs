using FlagsX0.Business.UseCases;
using FlagsX0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        
        return RedirectToAction("Create", new FlagViewModel());
        
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var listFlags = await _getFlagUseCase.Execute();
        
        return View(new FlagIndexViewModel()
        {
            Flags = listFlags
        });
    }
}