using System.Collections.Generic;
using TicTacToe.Data;
using TicTacToe.Services.Multiplayer.Contracts.Models;

namespace TicTacToe.Services.Multiplayer.Contracts;

public interface IMultiplayerModeDBService
{
    void SaveGameResult(MultiplayerMode gameResult);
    int GetTotalGameCount();
    List<MultiplayerMode> GetPaginatedGames(int page, int pageSize);
    List<PlayerPairStats> GetMultiplayerLeaderboard(int maxEntries = 20);
}