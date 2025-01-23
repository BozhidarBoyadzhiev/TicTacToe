using System;

namespace TicTacToe.Data;

public class ComputerLeaderboard
{    
    public Guid Id { get; set; }
    public string PlayerName { get; set; }
    public DateTime Date { get; set; }
    public string Winner { get; set; }
}