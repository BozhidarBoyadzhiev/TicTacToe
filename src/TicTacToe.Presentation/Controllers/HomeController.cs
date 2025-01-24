using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicTacToe.Presentation.Models;
using TicTacToe.Presentation.Models.ViewModels;
using TicTacToe.Services.Computer;
using TicTacToe.Services.Computer.Contracts;

namespace TicTacToe.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IComputerModeDBService computerModeDbService;

    public HomeController(IComputerModeDBService computerModeDbService)
    {
        this.computerModeDbService = computerModeDbService;
    }
    
    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("leaderboard")]
    public IActionResult Leaderboard()
    {
        var stats = this.computerModeDbService.GetComputerLeaderboard();
        return View(new LeaderboardViewModel { PlayerStats = stats });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}