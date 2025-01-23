using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicTacToe.Presentation.Models;
using TicTacToe.Services.Computer.Contracts;

namespace TicTacToe.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IComputerService computerService;

    public HomeController(
        ILogger<HomeController> logger,
        IComputerService computerService)
    {
        this.logger = logger;
        this.computerService = computerService;
    }

    [HttpGet("/")]
    public IActionResult Index()
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