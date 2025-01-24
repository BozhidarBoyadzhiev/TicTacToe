namespace TicTacToe.Services.Multiplayer.Contracts;

public class MultiplayerGameState
{
    public string Player1Name { get; set; }
    public string Player2Name { get; set; }
    public string Player1Color { get; set; }
    public string Player2Color { get; set; }
    public string CurrentPlayerSymbol { get; set; } = "X";
    public string[] Board { get; set; } = new string[9];
    public bool IsGameOver { get; set; }
    
    public string CurrentPlayerColor => 
        CurrentPlayerSymbol == "X" ? Player1Color : Player2Color;
}