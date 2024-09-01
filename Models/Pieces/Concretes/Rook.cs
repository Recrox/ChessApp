using Models;
using System.Drawing;

namespace ChessApp.Models.Pieces.Concretes;

public class Rook : Piece
{
    public Rook(Position position, Color color) : base(position, color)
    {
    }

    public override bool IsMovable(Position to)
    {
        // La tour se déplace horizontalement ou verticalement
        return (Position.X == to.X || Position.Y == to.Y);
    }

    public override string ToString()
    {
        return "R";
    }
}

