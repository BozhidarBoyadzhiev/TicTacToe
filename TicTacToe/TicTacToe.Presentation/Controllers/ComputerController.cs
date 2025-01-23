using Microsoft.AspNetCore.Mvc;
using TicTacToe.Services.Computer.Contracts;

namespace TicTacToe.Presentation.Controllers;

public class ComputerController : Controller
{
    private readonly IComputerService computerService;

    public ComputerController(IComputerService computerService)
    {
        this.computerService = computerService;
    }

    [HttpGet("computer")]
    public IActionResult Computer()
    {
        var gameState = this.computerService.InitializeGame();
        return View(gameState);
    }
    
    [HttpPost("computer/move")]
    public IActionResult PlayerMove([FromBody] GameStateDto gameState)
    {
        var updatedState = this.computerService.ProcessPlayerMove(gameState);
        return Json(updatedState);
    }
}