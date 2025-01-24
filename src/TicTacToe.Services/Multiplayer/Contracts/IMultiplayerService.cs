namespace TicTacToe.Services.Multiplayer.Contracts.Models;

public interface IMultiplayerService
{
    MultiplayerGameState StartNewGame(string player1Name, string player2Name);

    public (bool isValid, bool isGameOver, string winnerMessage,
        string currentSymbol, string currentColor, int[] winningCombination)
        ProcessMove(MultiplayerGameState gameState, int cellIndex);
}