using System.Threading.Tasks;
using TicTacToe.Data;

namespace TicTacToe.Services.Computer.Contracts;

public interface IComputerService
{
    GameStateDto InitializeGame();
    GameStateDto ProcessPlayerMove(GameStateDto gameState);
}