using System;
using System.Linq;
using TicTacToe.Services.Computer.Contracts;

namespace TicTacToe.Services.Computer;

public class ComputerService : IComputerService
{
    public GameStateDto InitializeGame()
    {
        return new GameStateDto
        {
            Board = new string[9],
            CurrentPlayer = "X",
            GameOver = false,
            Winner = null
        };
    }

    public GameStateDto ProcessPlayerMove(GameStateDto gameState)
    {
        // Player's move
        gameState.Board[gameState.LastMoveIndex] = "X";
        
        // Check if player wins
        if (CheckWinner(gameState.Board, "X"))
        {
            gameState.GameOver = true;
            gameState.Winner = "X";
            return gameState;
        }

        // Computer's move
        int computerMoveIndex = GenerateComputerMove(gameState.Board);
        gameState.Board[computerMoveIndex] = "O";

        // Check if computer wins
        if (CheckWinner(gameState.Board, "O"))
        {
            gameState.GameOver = true;
            gameState.Winner = "O";
        }
        
        // Check for draw
        if (gameState.Board.All(cell => cell != null))
        {
            gameState.GameOver = true;
        }

        return gameState;
    }

    private int GenerateComputerMove(string[] board)
    {
        var random = new Random();
        int moveIndex;
        do
        {
            moveIndex = random.Next(0, 9);
        } while (board[moveIndex] != null);

        return moveIndex;
    }

    private bool CheckWinner(string[] board, string player)
    {
        // Winning combinations
        int[][] winConditions = new int[][]
        {
            new int[] {0, 1, 2},
            new int[] {3, 4, 5},
            new int[] {6, 7, 8},
            new int[] {0, 3, 6},
            new int[] {1, 4, 7},
            new int[] {2, 5, 8},
            new int[] {0, 4, 8},
            new int[] {2, 4, 6}
        };

        return winConditions.Any(condition => 
            board[condition[0]] == player &&
            board[condition[1]] == player &&
            board[condition[2]] == player
        );
    }
}