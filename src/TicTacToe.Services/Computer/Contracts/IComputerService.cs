using System.Threading.Tasks;
using TicTacToe.Data;
using TicTacToe.Services.Computer.Contracts.Models;

namespace TicTacToe.Services.Computer.Contracts;

public interface IComputerService
{
    public ComputerGameState StartNewGame(string playerName, string playerColor, string playerSymbol);
    (bool isValid, bool isGameOver, string winnerMessage, int? computerIndex, int[] winningCombination) 
        ProcessMove(ComputerGameState computerGameState, int cellIndex);

    public int? MakeInitialComputerMove(ComputerGameState computerGameState);
}