using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicTacToe.Presentation.Models;
using TicTacToe.Presentation.Models.ViewModels;
using TicTacToe.Services.Computer;
using TicTacToe.Services.Computer.Contracts;
using TicTacToe.Services.Multiplayer.Contracts;

namespace TicTacToe.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IComputerModeDBService computerModeDbService;
    private readonly IMultiplayerModeDBService multiplayerModeDbService;
    public HomeController(
        IComputerModeDBService computerModeDbService,
        IMultiplayerModeDBService multiplayerModeDbService)
    {
        this.computerModeDbService = computerModeDbService;
        this.multiplayerModeDbService = multiplayerModeDbService;
    }
    
    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("leaderboard")]
    public IActionResult Leaderboard()
    {
        var computerStats = this.computerModeDbService.GetComputerLeaderboard();
        var multiplayerStats = this.multiplayerModeDbService.GetMultiplayerLeaderboard();
        
        return View(new LeaderboardViewModel { 
            ComputerPlayerStats = computerStats,
            MultiplayerStats = multiplayerStats
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}