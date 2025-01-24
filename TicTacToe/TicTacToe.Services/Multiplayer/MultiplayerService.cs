using System.Linq;
using TicTacToe.Services.Multiplayer.Contracts;

namespace TicTacToe.Services.Multiplayer;

public class MultiplayerService : IMultiplayerService
{
    private readonly int[][] winningCombinations =
    {
        new[] { 0, 1, 2 }, // Row 1
        new[] { 3, 4, 5 }, // Row 2
        new[] { 6, 7, 8 }, // Row 3
        new[] { 0, 3, 6 }, // Column 1
        new[] { 1, 4, 7 }, // Column 2
        new[] { 2, 5, 8 }, // Column 3
        new[] { 0, 4, 8 }, // Diagonal 1
        new[] { 2, 4, 6 } // Diagonal 2
    };
public MultiplayerGameState StartNewGame(string player1Name, string player2Name)
    {
        return new MultiplayerGameState
        {
            Player1Name = player1Name,
            Player2Name = player2Name,
            Board = new string[9],
            IsGameOver = false,
            CurrentPlayerSymbol = "X"
        };
    }

    public (
        bool isValid, 
        bool isGameOver, 
        string winnerMessage, 
        string currentSymbol, 
        string currentColor, 
        int[] winningCombination
        ) ProcessMove(MultiplayerGameState gameState, int cellIndex)
    {
        if (!IsMoveValid(gameState, cellIndex))
            return (false, false, null, null, null, null);

        // Store current color before switching players
        var playedColor = gameState.CurrentPlayerColor;
    
        gameState.Board[cellIndex] = gameState.CurrentPlayerSymbol;

        var (gameOver, winner, combination) = CheckWinner(gameState.Board);
        if (gameOver)
        {
            gameState.IsGameOver = true;
            return (true, true, GetWinnerMessage(gameState, winner), 
                null, null, combination);
        }

        if (IsBoardFull(gameState.Board))
            return (true, true, "Game Draw!", null, null, null);

        // Switch players after storing the played color
        gameState.CurrentPlayerSymbol = gameState.CurrentPlayerSymbol == "X" ? "O" : "X";
    
        return (true, false, null, 
            gameState.CurrentPlayerSymbol, 
            gameState.CurrentPlayerColor, 
            null);
    }
    
    private (bool, string, int[]) CheckWinner(string[] board)
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

    private bool IsMoveValid(MultiplayerGameState state, int cellIndex) => 
        !state.IsGameOver && 
        cellIndex >= 0 && cellIndex < 9 &&
        string.IsNullOrEmpty(state.Board[cellIndex]);

    private bool IsBoardFull(string[] board) => board.All(c => !string.IsNullOrEmpty(c));

    private string GetWinnerMessage(MultiplayerGameState state, string winnerSymbol)
    {
        return winnerSymbol == "X" 
            ? $"{state.Player1Name} wins!" 
            : $"{state.Player2Name} wins!";
    }
}