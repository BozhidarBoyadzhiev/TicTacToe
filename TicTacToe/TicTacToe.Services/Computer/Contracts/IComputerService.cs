using System.Threading.Tasks;
using TicTacToe.Data;

namespace TicTacToe.Services.Computer.Contracts;

public interface IComputerService
{
    public GameState StartNewGame(string playerName, string playerColor, string playerSymbol);
    (bool isValid, bool isGameOver, string winnerMessage, int? computerIndex, int[] winningCombination) 
        ProcessMove(GameState gameState, int cellIndex);

    public int? MakeInitialComputerMove(GameState gameState);
}