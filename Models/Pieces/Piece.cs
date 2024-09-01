using System.Drawing;
using Models;

namespace ChessApp.Models.Pieces;

public abstract class Piece
{
    public Position Position { get; set; } = null!;
    public Color Color { get; set; }

    protected Piece(Position position, Color color)
    {
        Position = position;
        Color = color;
    }

    public abstract override string ToString();
    public abstract bool IsMovable(Position to);
}
