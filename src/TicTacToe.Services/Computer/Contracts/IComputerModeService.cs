using System.Collections.Generic;
using TicTacToe.Data;
using TicTacToe.Presentation.Models;

namespace TicTacToe.Services.Computer.Contracts;

public interface IComputerModeDBService
{
    void SaveGameResult(ComputerMode gameResult);
    public int GetTotalGameCount();
    public List<ComputerMode> GetPaginatedGames(int page, int pageSize);
    public List<PlayerStats> GetComputerLeaderboard(int maxEntries = 20);
}