using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Presentation.Models;

namespace TicTacToe.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet("computer")]
    public IActionResult Computer()
    {
        return View();
    }
    
    [HttpGet("multiplayer")]
    public IActionResult Multiplayer()
    {
        return View();
    } 
    
    [HttpGet("leaderboard")]
    public IActionResult Leaderboard()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}