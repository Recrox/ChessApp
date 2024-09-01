using Models;
using System.Drawing;

namespace ChessApp.Models.Pieces.Concretes;

public class King : Piece
{
    public King(Position position, Color color) : base(position, color)
    {
    }

    public override bool IsMovable(Position to)
    {
        // Le roi se déplace d'une case dans toutes les directions
        var dx = Math.Abs(Position.X - to.X);
        var dy = Math.Abs(Position.Y - to.Y);
        return (dx <= 1 && dy <= 1);
    }

    public override string ToString()
    {
        return "9";
    }
}

