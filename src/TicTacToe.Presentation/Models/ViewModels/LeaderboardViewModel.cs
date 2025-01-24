using System.Collections.Generic;
using TicTacToe.Services.Computer.Contracts.Models;
using TicTacToe.Services.Multiplayer.Contracts.Models;

namespace TicTacToe.Presentation.Models.ViewModels;

public class LeaderboardViewModel
{
    public List<PlayerStats> ComputerPlayerStats { get; set; }
    public List<PlayerPairStats> MultiplayerStats { get; set; }
}