using Models;
using System.Drawing;

namespace ChessApp.Models.Pieces.Concretes;

public class Pawn : Piece
{
    public Pawn(Position position, Color color) : base(position, color)
    {
    }

    public override bool IsMovable(Position to)
    {
        // La direction du mouvement dépend de la couleur du pion
        int direction = Color == Color.White ? 1 : -1;

        // Déplacement d'une case en avant
        if (Position.X == to.X && Position.Y + direction == to.Y)
        {
            return true; // L'emplacement doit être vide, cette vérification est effectuée ailleurs
        }

        // Déplacement de deux cases depuis la position initiale
        if (Position.X == to.X && Position.Y == (Color == Color.White ? 1 : 6) &&
            Position.Y + 2 * direction == to.Y)
        {
            return true; // L'emplacement doit être vide, cette vérification est effectuée ailleurs
        }

        // Capture en diagonale
        if (Math.Abs(Position.X - to.X) == 1 && Position.Y + direction == to.Y)
        {
            return true; // La case doit contenir une pièce adverse, cette vérification est effectuée ailleurs
        }

        return false;
    }

    public override string ToString()
    {
        return Color == Color.White ? "♙" : "♟";
    }
}

