using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Presentation.Models;
using TicTacToe.Services.Multiplayer.Contracts;

namespace TicTacToe.Presentation.Controllers;

[Route("[controller]")]
public class MultiplayerController : Controller
{
    private readonly IMultiplayerService _multiplayerService;

    public MultiplayerController(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
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
    
        // Store the color before processing the move
        var playedColor = gameState.CurrentPlayerSymbol == "X" 
            ? gameState.Player1Color 
            : gameState.Player2Color;

        var (isValid, isGameOver, message, currentSymbol, currentColor, combination) = 
            _multiplayerService.ProcessMove(gameState, request.CellIndex);

        HttpContext.Session.SetString("MultiplayerState", 
            JsonSerializer.Serialize(gameState));

        return Json(new {
            isValid,
            isGameOver,
            message,
            playedSymbol = gameState.Board[request.CellIndex],
            playedColor, // Use the stored color before the turn changed
            currentSymbol,
            currentColor,
            combination
        });
    }
}