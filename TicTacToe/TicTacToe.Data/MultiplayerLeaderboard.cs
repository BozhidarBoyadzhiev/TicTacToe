using System;

namespace TicTacToe.Data;

public class MultiplayerLeaderboard
{
    public Guid Id { get; set; }
    public string Player1Name { get; set; }
    public string Player2Name { get; set; }
    public DateTime Date { get; set; }
    public string Winner { get; set; }
}