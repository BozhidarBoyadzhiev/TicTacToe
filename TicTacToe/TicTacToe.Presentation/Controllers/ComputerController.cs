using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Presentation.Models;
using TicTacToe.Services.Computer.Contracts;

namespace TicTacToe.Presentation.Controllers;

[Route("[controller]")]
public class ComputerController : Controller
{
    private readonly IComputerService computerService;

    public ComputerController(IComputerService computerService)
    {
        this.computerService = computerService;
    }

    public IActionResult Computer()
    {
        return View();
    }

    [HttpPost("NewGame")]
    public IActionResult NewGame([FromBody] GameSettings settings)
    {
        try
        {
            var gameState = this.computerService.StartNewGame(
                settings.PlayerName, 
                settings.PlayerColor,
                settings.PlayerSymbol
            );
        
            HttpContext.Session.SetString("GameState", JsonSerializer.Serialize(gameState));
            
            int? computerFirstMove = null;
            if (gameState.PlayerSymbol == "O")
            {
                computerFirstMove = this.computerService.MakeInitialComputerMove(gameState);
                HttpContext.Session.SetString("GameState", JsonSerializer.Serialize(gameState));
            }

            return Json(new { 
                success = true,
                computerFirstMove,
                computerSymbol = gameState.ComputerSymbol
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("MakeMove")]
    public IActionResult MakeMove([FromBody] MoveRequest request)
    {
        try
        {
            var gameStateJson = HttpContext.Session.GetString("GameState");
            if (string.IsNullOrEmpty(gameStateJson))
                return BadRequest(new { error = "Game not started" });

            var gameState = JsonSerializer.Deserialize<GameState>(gameStateJson);
            var result = this.computerService.ProcessMove(gameState, request.CellIndex);
        
            HttpContext.Session.SetString("GameState", JsonSerializer.Serialize(gameState));

            return Json(new
            {
                isValid = result.isValid,
                isGameOver = result.isGameOver,
                winnerMessage = result.winnerMessage,
                computerIndex = result.computerIndex,
                playerSymbol = gameState.PlayerSymbol,
                computerSymbol = gameState.ComputerSymbol,
                playerColor = gameState.PlayerColor,
                winningCombination = result.winningCombination
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}