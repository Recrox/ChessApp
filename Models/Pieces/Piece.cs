using System.Drawing;
using Models;

namespace ChessApp.Models.Pieces;

public abstract class Piece
{
    public Position Position { get; set; } = null!;
    public Color Color { get; set; }

    public abstract override string ToString();
}
