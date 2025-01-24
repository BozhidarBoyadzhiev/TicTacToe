using System;
using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Data;

public class MultiplayerMode
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Player1Name { get; set; }
    public string Player2Name { get; set; }
    public string Player1Color { get; set; }
    public string Player2Color { get; set; }
    public string Result { get; set; }
}