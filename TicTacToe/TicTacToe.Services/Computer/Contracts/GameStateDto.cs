namespace TicTacToe.Services.Computer.Contracts;

public class GameStateDto
{
    public string[] Board { get; set; }
    public string CurrentPlayer { get; set; }
    public bool GameOver { get; set; }
    public string Winner { get; set; }
    public int LastMoveIndex { get; set; }
}