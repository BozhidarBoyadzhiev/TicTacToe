using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Services.Computer.Contracts;
using TicTacToe.Services.Computer.Contracts.Models;

namespace TicTacToe.Services.Computer;

public class ComputerModeDBService : IComputerModeDBService
{
    private readonly AppDbContext dbcontext;

    public ComputerModeDBService(AppDbContext context)
    {
        this.dbcontext = context;
    }

    public void SaveGameResult(ComputerMode gameResult)
    {
        gameResult.Date = DateTime.UtcNow;
        this.dbcontext.ComputerModes.Add(gameResult);
        this.dbcontext.SaveChanges();
    }

    public int GetTotalGameCount()
    {
        return this.dbcontext.ComputerModes.Count();
    }

    public List<ComputerMode> GetPaginatedGames(int page, int pageSize)
    {
        return this.dbcontext.ComputerModes
            .OrderByDescending(g => g.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToList();
    }
    
    public List<PlayerStats> GetComputerLeaderboard(int maxEntries = 20)
    {
        var rawData = this.dbcontext.ComputerModes
            .AsNoTracking()
            .GroupBy(g => new { g.PlayerName, g.PlayerColor })
            .Select(g => new 
            {
                g.Key.PlayerName,
                g.Key.PlayerColor,
                Wins = g.Count(x => x.Result == "Win"),
                Losses = g.Count(x => x.Result == "Loss"),
                Draws = g.Count(x => x.Result == "Draw")
            })
            .ToList();

        var results = rawData
            .Select(x => {
                var totalGames = x.Wins + x.Losses + x.Draws;
                return new PlayerStats
                {
                    PlayerName = x.PlayerName,
                    PlayerColor = x.PlayerColor,
                    Wins = x.Wins,
                    Losses = x.Losses,
                    Draws = x.Draws,
                    TotalGames = totalGames,
                    WinPercentage = totalGames > 0 
                        ? (x.Wins * 100.0) / totalGames 
                        : 0,
                    // New ranking score that combines win rate and game volume
                    Score = totalGames > 0 
                        ? (x.Wins * 100.0) / totalGames * Math.Log(totalGames + 1) 
                        : 0
                };
            })
            .OrderByDescending(p => p.Score)
            .ThenByDescending(p => p.TotalGames)
            .Take(maxEntries)
            .ToList();

        return results;
    }
}