using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Data;
using TicTacToe.Presentation.Models;
using TicTacToe.Services.Computer.Contracts;
using TicTacToe.Services.Computer.Contracts.Models;

namespace TicTacToe.Presentation.Controllers;

[Route("[controller]")]
public class ComputerController : Controller
{
    private readonly IComputerService computerService;
    private readonly IComputerModeDBService computerDbService;

    public ComputerController(
        IComputerService computerService,
        IComputerModeDBService computerDbService)
    {
        this.computerService = computerService;
        this.computerDbService = computerDbService;
    }

    public IActionResult Computer()
    {
        return View();
    }

    [HttpPost("NewGame")]
    public IActionResult NewGame([FromBody] ComputerGameSettings settings)
    {
        try
        {
            var gameState = this.computerService.StartNewGame(
                settings.PlayerName, 
                settings.PlayerColor,
                settings.PlayerSymbol
            );
        
            HttpContext.Session.SetString("ComputerGameState", JsonSerializer.Serialize(gameState));
            
            int? computerFirstMove = null;
            if (gameState.PlayerSymbol == "O")
            {
                computerFirstMove = this.computerService.MakeInitialComputerMove(gameState);
                HttpContext.Session.SetString("ComputerGameState", JsonSerializer.Serialize(gameState));
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
            var gameStateJson = HttpContext.Session.GetString("ComputerGameState");
            if (string.IsNullOrEmpty(gameStateJson))
                return BadRequest(new { error = "Game not started" });

            var gameState = JsonSerializer.Deserialize<ComputerGameState>(gameStateJson);
            var result = this.computerService.ProcessMove(gameState, request.CellIndex);
                
            if (result.isGameOver)
            {
                var gameResult = new ComputerMode
                {
                    PlayerName = gameState.PlayerName,
                    PlayerColor = gameState.PlayerColor,
                    Result = result.winnerMessage.Contains("wins") 
                        ? (result.winnerMessage.Contains("Computer") ? "Loss" : "Win")
                        : "Draw"
                };
                    
                this.computerDbService.SaveGameResult(gameResult);
            }

            HttpContext.Session.SetString("ComputerGameState", JsonSerializer.Serialize(gameState));

            return Json(new
            {
                result.isValid,
                result.isGameOver,
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
    
    [HttpGet("GetHistory")]
    public IActionResult GetHistory(int page = 1, int pageSize = 10)
    {
        try
        {
            var totalCount = this.computerDbService.GetTotalGameCount();
            var games = this.computerDbService.GetPaginatedGames(page, pageSize);
        
            return Json(new 
            {
                games = games.Select(g => new 
                {
                    date = g.Date,
                    playerName = g.PlayerName,
                    playerColor = g.PlayerColor,
                    result = g.Result
                }),
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}