using System;
using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Data;

public class ComputerMode
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string PlayerName { get; set; }
    public string PlayerColor { get; set; }
    public string Result { get; set; }
}