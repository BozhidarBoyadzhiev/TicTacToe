using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Presentation.Models;

public class GameSettings
{
    public string PlayerName { get; set; }
    public string PlayerColor { get; set; }
    public string PlayerSymbol { get; set; } = "X";
}