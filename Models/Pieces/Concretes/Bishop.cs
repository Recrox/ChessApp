using Models;
using System.Drawing;

namespace ChessApp.Models.Pieces.Concretes;
public class Bishop : Piece
{
    public Bishop(Position position, Color color) : base(position, color)
    {
    }

    public override bool IsMovable(Position to)
    {
        // Le fou se déplace en diagonale
        return Math.Abs(Position.X - to.X) == Math.Abs(Position.Y - to.Y);
    }

    public override string ToString()
    {
        return "B";
    }
}

