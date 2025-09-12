using FlagsX0.Business.UseCases;
using FlagsX0.DTOs;
using FlagsX0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlagsX0.Controllers;

[Authorize]
[Route("[controller]")]
public class FlagsController(
    AddFlagUseCase addFlagUseCase,
    GetFlagsUseCase getFlagUseCase,
    GetSingleFlagUseCase getSingleFlagUseCase,
    UpdateFlagUseCase updateFlagUseCase,
    DeleteFlagUseCase deleteFlagUseCase) : Controller
{
    private readonly AddFlagUseCase _addFlagUseCase = addFlagUseCase;
    private readonly DeleteFlagUseCase _deleteFlagUseCase = deleteFlagUseCase;
    private readonly GetFlagsUseCase _getFlagUseCase = getFlagUseCase;
    private readonly GetSingleFlagUseCase _getSingleFlagUseCase = getSingleFlagUseCase;
    private readonly UpdateFlagUseCase _updateFlagUseCase = updateFlagUseCase;

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

    [HttpGet("{flagName}")]
    public async Task<IActionResult> GetSingleFlag(string flagName, string? message)
    {
        var singleFlag = await _getSingleFlagUseCase.Execute(flagName);


        return View("SingleFlag", new SingleFlagViewModel
        {
            Flag = singleFlag.Value,
            Message = message
        });
    }

    [HttpPost("{flagName}")]
    public async Task<IActionResult> UpdateFlag(FlagDto flag)
    {
        var updatedFlag = await _updateFlagUseCase.Execute(flag);

        if (updatedFlag.Success)
            return View("SingleFlag", new SingleFlagViewModel
            {
                Flag = updatedFlag.Value,
                Message = "Flag Updated Correctly"
            });

        return View("SingleFlag", new SingleFlagViewModel
        {
            Flag = flag,
            Error = updatedFlag.Errors.First().Message
        });
    }

    [HttpGet("delete/{flagName}")]
    public async Task<IActionResult> DeleteFlag(string flagName)
    {
        var isDeleted = await _deleteFlagUseCase.Execute(flagName);

        if (isDeleted.Success) return RedirectToAction("");

        return RedirectToAction("GetSingleFlag", new
        {
            flagName,
            message = "Updated Correctly"
        });
    }
}