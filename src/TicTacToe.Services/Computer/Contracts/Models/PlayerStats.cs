using System;

namespace TicTacToe.Presentation.Models;

public class PlayerStats
{
    public string PlayerName { get; set; }
    public string PlayerColor { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public int TotalGames { get; set; }
    public double WinPercentage {get; set;}
    
    public double Score { get; set; }
}