using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Data;
using TicTacToe.Presentation.Models;
using TicTacToe.Services.Multiplayer.Contracts;
using TicTacToe.Services.Multiplayer.Contracts.Models;

namespace TicTacToe.Presentation.Controllers;

[Route("[controller]")]
public class MultiplayerController : Controller
{
    private readonly IMultiplayerService multiplayerService;
    private readonly IMultiplayerModeDBService multiplayerModeDBService;

    public MultiplayerController(
        IMultiplayerService multiplayerService,
        IMultiplayerModeDBService multiplayerModeDBService)
    {
        this.multiplayerService = multiplayerService;
        this.multiplayerModeDBService = multiplayerModeDBService;
    }
    
    public IActionResult Multiplayer()
    {
        return View();
    }

    [HttpPost("NewGame")]
    public IActionResult NewGame([FromBody] MultiplayerSettings settings)
    {
        var gameState = new MultiplayerGameState
        {
            Player1Name = settings.Player1Name,
            Player2Name = settings.Player2Name,
            Player1Color = settings.Player1Color,
            Player2Color = settings.Player2Color,
            Board = new string[9],
            IsGameOver = false,
            CurrentPlayerSymbol = "X"
        };

        HttpContext.Session.SetString("MultiplayerState", 
            JsonSerializer.Serialize(gameState));
    
        return Json(new { success = true });
    }

    [HttpPost("MakeMove")]
    public IActionResult MakeMove([FromBody] MoveRequest request)
    {
        var stateJson = HttpContext.Session.GetString("MultiplayerState");
        if (string.IsNullOrEmpty(stateJson))
            return BadRequest(new { error = "Game not started" });

        var gameState = JsonSerializer.Deserialize<MultiplayerGameState>(stateJson);
        
        var (isValid, isGameOver, message, currentSymbol, currentColor, combination) = 
            this.multiplayerService.ProcessMove(gameState, request.CellIndex);

        if (isGameOver)
        {
            var gameResult = new MultiplayerMode
            {
                Player1Name = gameState.Player1Name,
                Player2Name = gameState.Player2Name,
                Player1Color = gameState.Player1Color,
                Player2Color = gameState.Player2Color,
                Result = message.Contains("wins") 
                    ? (message.Contains(gameState.Player1Name) ? "Player1 Win" : "Player2 Win")
                    : "Draw"
            };
            
            this.multiplayerModeDBService.SaveGameResult(gameResult);
        }

        HttpContext.Session.SetString("MultiplayerState", 
            JsonSerializer.Serialize(gameState));

        return Json(new {
            isValid,
            isGameOver,
            message,
            playedSymbol = gameState.Board[request.CellIndex],
            playedColor = currentSymbol == "X" ? gameState.Player1Color : gameState.Player2Color,
            currentSymbol,
            currentColor,
            combination
        });
    }
    
    [HttpGet("GetHistory")]
    public IActionResult GetHistory(int page = 1, int pageSize = 10)
    {
        try
        {
            var totalCount = this.multiplayerModeDBService.GetTotalGameCount();
            var games = this.multiplayerModeDBService.GetPaginatedGames(page, pageSize);
    
            return Json(new 
            {
                games = games.Select(g => new 
                {
                    date = g.Date,
                    player1Name = g.Player1Name,
                    player2Name = g.Player2Name,
                    player1Color = g.Player1Color,
                    player2Color = g.Player2Color,
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