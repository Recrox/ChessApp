using Models;
using System.Drawing;

namespace ChessApp.Models.Pieces.Concretes;

public class Knight : Piece
{
    public Knight(Position position, Color color) : base(position, color)
    {
    }

    public override bool IsMovable(Position to)
    {
        // Le cavalier se déplace en "L" : 2 cases dans une direction et 1 case perpendiculaire
        var dx = Math.Abs(Position.X - to.X);
        var dy = Math.Abs(Position.Y - to.Y);
        return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
    }

    public override string ToString()
    {
        return "K";
    }
}

