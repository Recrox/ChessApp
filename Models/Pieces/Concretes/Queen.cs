using Models;
using System.Drawing;

namespace ChessApp.Models.Pieces.Concretes;

public class Queen : Piece
{
    public Queen(Position position, Color color) : base(position, color)
    {
    }

    public override bool IsMovable(Position to)
    {
        // La reine se déplace comme une tour ou un fou
        return (Position.X == to.X || Position.Y == to.Y) ||
               (Math.Abs(Position.X - to.X) == Math.Abs(Position.Y - to.Y));
    }

    public override string ToString()
    {
        return Color == Color.White ? "♕" : "♛";
    }
}

