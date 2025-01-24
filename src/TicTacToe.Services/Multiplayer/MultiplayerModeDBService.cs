using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Services.Multiplayer.Contracts;
using TicTacToe.Services.Multiplayer.Contracts.Models;

namespace TicTacToe.Services.Multiplayer;

public class MultiplayerModeDBService : IMultiplayerModeDBService
{
    private readonly AppDbContext dbContext;

    public MultiplayerModeDBService(AppDbContext context)
    {
        dbContext = context;
    }

    public void SaveGameResult(MultiplayerMode gameResult)
    {
        gameResult.Date = DateTime.UtcNow;
        dbContext.MultiplayerModes.Add(gameResult);
        dbContext.SaveChanges();
    }

    public int GetTotalGameCount()
    {
        return dbContext.MultiplayerModes.Count();
    }

    public List<MultiplayerMode> GetPaginatedGames(int page, int pageSize)
    {
        return dbContext.MultiplayerModes
            .OrderByDescending(g => g.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToList();
    }

    public List<PlayerPairStats> GetMultiplayerLeaderboard(int maxEntries = 20)
    {
        var rawData = dbContext.MultiplayerModes
            .AsNoTracking()
            .ToList()
            .SelectMany(g => new[] 
            {
                new { 
                    Player = g.Player1Name, 
                    Color = g.Player1Color,
                    Wins = g.Result == "Player1 Win" ? 1 : 0,
                    Losses = g.Result == "Player2 Win" ? 1 : 0,
                    Draws = g.Result == "Draw" ? 1 : 0
                },
                new {
                    Player = g.Player2Name,
                    Color = g.Player2Color,
                    Wins = g.Result == "Player2 Win" ? 1 : 0,
                    Losses = g.Result == "Player1 Win" ? 1 : 0,
                    Draws = g.Result == "Draw" ? 1 : 0
                }
            })
            .ToList();

        var playerStats = rawData
            .GroupBy(x => new { x.Player, x.Color }) // Group by both name and color
            .Select(g => new PlayerPairStats
            {
                PlayerName = g.Key.Player,
                PlayerColor = g.Key.Color,
                Wins = g.Sum(x => x.Wins),
                Losses = g.Sum(x => x.Losses),
                Draws = g.Sum(x => x.Draws),
                TotalGames = g.Count(),
                WinPercentage = g.Count() > 0 
                    ? (g.Sum(x => x.Wins) * 100.0) / g.Count() 
                    : 0
            })
            .OrderByDescending(p => p.WinPercentage)
            .ThenByDescending(p => p.TotalGames)
            .Take(maxEntries)
            .ToList();

        return playerStats;
    }
}