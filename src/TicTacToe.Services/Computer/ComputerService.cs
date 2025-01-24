using System;
using System.Linq;
using TicTacToe.Data;
using TicTacToe.Services.Computer.Contracts;
using TicTacToe.Services.Computer.Contracts.Models;

namespace TicTacToe.Services.Computer;

public class ComputerService : IComputerService
{
    private readonly IComputerModeDBService computerModeDBService;
    
    private readonly int[][] winningCombinations = 
    {
        new[] {0, 1, 2}, // Row 1
        new[] {3, 4, 5}, // Row 2
        new[] {6, 7, 8}, // Row 3
        new[] {0, 3, 6}, // Column 1
        new[] {1, 4, 7}, // Column 2
        new[] {2, 5, 8}, // Column 3
        new[] {0, 4, 8}, // Diagonal 1
        new[] {2, 4, 6}  // Diagonal 2
    };
    
    public ComputerService(IComputerModeDBService computerModeDBService)
    {
        this.computerModeDBService = computerModeDBService;
    }

    public ComputerGameState StartNewGame(string playerName, string playerColor, string playerSymbol)
    {
        return new ComputerGameState
        {
            PlayerName = playerName,
            PlayerColor = playerColor,
            PlayerSymbol = playerSymbol,
            IsPlayerTurn = (playerSymbol == "X"), // Player starts if X, computer if O
            Board = new string[9],
            IsGameOver = false
        };
    }

    public (
        bool isValid, 
        bool isGameOver, 
        string winnerMessage, 
        int? computerIndex, 
        int[] winningCombination) 
        ProcessMove(ComputerGameState computerGameState, int cellIndex)
    {
        // Validate move
        if (!IsValidMove(computerGameState, cellIndex))
            return (false, false, null, null, null);

        // Player's move
        computerGameState.Board[cellIndex] = computerGameState.PlayerSymbol;
        computerGameState.IsPlayerTurn = false;

        // Check for player win
        var (gameOver, winnerSymbol, combination) = CheckWinner(computerGameState.Board);
        if (gameOver)
            return HandleGameResult(computerGameState, winnerSymbol, combination);

        // Check for draw
        if (IsBoardFull(computerGameState.Board))
            return (true, true, "Game Draw!", null, null);

        // Computer's move (only if game isn't over)
        int computerMove = GetComputerMove(computerGameState.Board);
        computerGameState.Board[computerMove] = computerGameState.ComputerSymbol;
        computerGameState.IsPlayerTurn = true;

        // Check for computer win
        (gameOver, winnerSymbol, combination) = CheckWinner(computerGameState.Board);
        return gameOver 
            ? HandleGameResult(computerGameState, winnerSymbol, combination, computerMove) 
            : (true, false, null, computerMove, null);
    }
    
    public int? MakeInitialComputerMove(ComputerGameState computerGameState)
    {
        if (computerGameState.PlayerSymbol != "O") return null;
    
        var computerMove = GetComputerMove(computerGameState.Board);
        computerGameState.Board[computerMove] = computerGameState.PlayerSymbol;
        computerGameState.IsPlayerTurn = true;
        return computerMove;
    }
    
    private void SaveGameResult(ComputerGameState state, string result)
    { 
        this.computerModeDBService.SaveGameResult(new ComputerMode
        {
            PlayerName = state.PlayerName,
            PlayerColor = state.PlayerColor,
            Result = result
        });
    }
    
    private (bool, bool, string, int?, int[]) HandleGameResult(
        ComputerGameState state, 
        string winnerSymbol, 
        int[] combination, 
        int? computerMove = null)
    {
        state.IsGameOver = true;
        string message = (winnerSymbol == state.PlayerSymbol) 
            ? $"{state.PlayerName} wins!" 
            : "Computer wins!";
        return (true, true, message, computerMove, combination);
    }

    private bool IsValidMove(ComputerGameState state, int cellIndex)
    {
        return cellIndex is >= 0 and <= 8 &&
               !state.IsGameOver &&
               (state.IsPlayerTurn && state.Board[cellIndex] == null);
    }

    private (bool isGameOver, string winnerSymbol, int[] combination) CheckWinner(string[] board)
    {
        foreach (var combo in this.winningCombinations)
        {
            if (!string.IsNullOrEmpty(board[combo[0]]) && 
                board[combo[0]] == board[combo[1]] && 
                board[combo[1]] == board[combo[2]])
            {
                return (true, board[combo[0]], combo);
            }
        }
        return (false, null, null);
    }

    private int GetComputerMove(string[] board)
    {
        var emptyCells = board.Select((cell, idx) => new { cell, idx })
            .Where(x => x.cell == null)
            .Select(x => x.idx)
            .ToList();
        return emptyCells[new Random().Next(emptyCells.Count)];
    }

    private bool IsBoardFull(string[] board) => board.All(c => !string.IsNullOrEmpty(c));
}