using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Presentation.Models;

public class MoveRequest
{
    [Range(0, 8, ErrorMessage = "Invalid cell index")]
    public int CellIndex { get; set; }
}