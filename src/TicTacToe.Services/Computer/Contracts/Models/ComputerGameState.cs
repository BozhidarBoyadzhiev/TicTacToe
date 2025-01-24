namespace TicTacToe.Services.Computer.Contracts.Models;

public class ComputerGameState
{
    public string[] Board { get; set; } = new string[9];
    public string PlayerName { get; set; }
    public string PlayerColor { get; set; }
    public string PlayerSymbol { get; set; }
    public string ComputerSymbol => PlayerSymbol == "X" ? "O" : "X";
    public bool IsPlayerTurn { get; set; }
    public bool IsGameOver { get; set; }
}