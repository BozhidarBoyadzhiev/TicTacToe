using System;
using System.Linq;
using TicTacToe.Services.Computer.Contracts;

namespace TicTacToe.Services.Computer;

public class ComputerService : IComputerService
{
    private readonly int[][] _winningCombinations = 
    {
        new[] {0, 1, 2}, new[] {3, 4, 5}, new[] {6, 7, 8},
        new[] {0, 3, 6}, new[] {1, 4, 7}, new[] {2, 5, 8},
        new[] {0, 4, 8}, new[] {2, 4, 6}
    };

    public GameState StartNewGame(string playerName, string playerColor, string playerSymbol)
    {
        return new GameState
        {
            PlayerName = playerName,
            PlayerColor = playerColor,
            PlayerSymbol = playerSymbol,
            Board = new string[9],
            IsPlayerTurn = playerSymbol == "X",
            IsGameOver = false
        };
    }

    public (bool isValid, bool isGameOver, string winnerMessage, int? computerIndex, int[] winningCombination) 
        ProcessMove(GameState gameState, int cellIndex)
    {
        if (!IsMoveValid(gameState, cellIndex))
            return (false, false, null, null, null)!;
        
        gameState.Board[cellIndex] = gameState.PlayerSymbol;
        gameState.IsPlayerTurn = false;

        var (gameOver, winner, combination) = CheckWinner(gameState.Board);
        if (gameOver)
            return HandleGameResult(gameState, winner, combination);

        if (IsBoardFull(gameState.Board))
            return (true, true, "Game Draw!", null, null)!;


        var computerMove = GetComputerMove(gameState.Board);
        gameState.Board[computerMove] = gameState.ComputerSymbol;
        gameState.IsPlayerTurn = true;

        (gameOver, winner, combination) = CheckWinner(gameState.Board);
        return (gameOver 
            ? HandleGameResult(gameState, winner, combination, computerMove) 
            : (true, false, null, computerMove, null))!;
    }
    
    public int? MakeInitialComputerMove(GameState gameState)
    {
        if (gameState.PlayerSymbol != "O") return null;
    
        var computerMove = GetComputerMove(gameState.Board);
        gameState.Board[computerMove] = gameState.PlayerSymbol;
        gameState.IsPlayerTurn = true;
        return computerMove;
    }
    
    private (bool, bool, string, int?, int[]) HandleGameResult(
        GameState state, 
        string winner, 
        int[] combination, 
        int? computerMove = null)
    {
        state.IsGameOver = true;
        var message = winner == state.PlayerSymbol 
            ? $"{state.PlayerName} wins!" 
            : "Computer wins!";
        return (true, true, message, computerMove, combination);
    }

    private bool IsMoveValid(GameState gameState, int cellIndex) => 
        !gameState.IsGameOver && 
        gameState.IsPlayerTurn && 
        gameState.Board[cellIndex] == null;

    private (bool, string, int[]) CheckWinner(string[] board)
    {
        foreach (var combo in _winningCombinations)
        {
            if (!string.IsNullOrEmpty(board[combo[0]]) && 
                board[combo[0]] == board[combo[1]] && 
                board[combo[1]] == board[combo[2]])
            {
                return (true, board[combo[0]], combo);
            }
        }
        return (false, null, null)!;
    }

    private int GetComputerMove(string[] board)
    {
        var emptyCells = board.Select((cell, idx) => new { cell, idx })
                            .Where(x => x.cell == null)
                            .Select(x => x.idx)
                            .ToList();
        return emptyCells[new Random().Next(emptyCells.Count)];
    }

    private bool IsBoardFull(string[] board) => board.All(c => c != null);
}