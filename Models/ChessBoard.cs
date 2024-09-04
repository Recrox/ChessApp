using ChessApp.Models.Exceptions;
using ChessApp.Models.Pieces;
using ChessApp.Models.Pieces.Concretes;
using System.Drawing;

namespace Models;

public class ChessBoard
{
    private const int COLUMN_SIZE = 8;
    private const int ROW_SIZE = 8;

    private Piece?[,] Board;
    public Player[] Players { get; set; }
    private Player currentPLayer;

    public ChessBoard()
    {
        this.Board = new Piece[COLUMN_SIZE, ROW_SIZE];
        this.Players = new Player[2];
    }

    public void StartGame()
    {
        InitPlayers();
        InitChessBoard();
        this.currentPLayer = this.Players[0];

        while (!GameIsOver())
        {
            ShowChessBoard();
            CurrentPlayerChooseAMove();
            SwitchTurn();
        }
    }

    private void CurrentPlayerChooseAMove()
    {
        while (true)
        {
            try
            {
                Position from = AskFromPos();
                Position to = AskToPos();

                // Tenter de déplacer la pièce
                MovePiece(from, to);

                // Si le mouvement est réussi, sortir de la boucle
                Console.WriteLine("Mouvement réussi !");
                break;
            }
            catch (PieceDontExistException)
            {
                Console.WriteLine("La pièce n'existe pas à la position spécifiée. Veuillez essayer à nouveau.");
            }
            catch (CantMoveException)
            {
                Console.WriteLine("Le mouvement n'est pas autorisé. Veuillez essayer à nouveau.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Format de position invalide. Assurez-vous d'utiliser le format 'LettreChiffre'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
            }
        }
    }

    private Position AskToPos()
    {
        // Demander la position d'arrivée
        Console.WriteLine("Entrez la position d'arrivée (format: LettreChiffre) :");
        var toInput = Console.ReadLine();
        var to = new Position(toInput); // Utiliser le constructeur avec une chaîne
        return to;
    }

    private Position AskFromPos()
    {
        // Demander la position de départ
        Console.WriteLine("Entrez la position de départ (format: LettreChiffre) :");
        var fromInput = Console.ReadLine();
        var from = new Position(fromInput); // Utiliser le constructeur avec une chaîne
        return from;
    }

    private void MovePiece(Position from, Position to)
    {
        //Check avant Déplacement
        var pieceToMove = this.GetPieceFromBoard(from) ?? throw new PieceDontExistException();
        Console.WriteLine($"Pièce sélectionnée : {pieceToMove.ToString()} à la position {from}");

        if (pieceToMove.Color != this.currentPLayer.Color)
            throw new NotMyPieceException();

        if (!pieceToMove.IsMovable(to))
            throw new CantMoveException("Déplacement incorrect");

        if (this.IsPieceOnTrajectory(from, to))
            throw new PieceBetweenException("Une pièce bloque la trajectoire.");

        Piece? pieceAtTarget = GetPieceFromBoard(to);
        // Si c'est une prise de pièce.
        if (pieceAtTarget is not null)
            EatPiece(to, pieceToMove, pieceAtTarget);

        // Déplacement
        this.Board[to.X, to.Y] = pieceToMove;
        this.Board[from.X, from.Y] = null;
        pieceToMove.Position = to; // Met à jour la position de la pièce
    }

    private void EatPiece(Position to, Piece pieceToMove, Piece pieceAtTarget)
    {
        Console.WriteLine($"Pièce à la position d'arrivée : {pieceAtTarget.ToString()} à la position {to}");
        if (pieceAtTarget.Color == pieceToMove.Color)
            throw new SameColorException(); // Ne peut pas se déplacer sur une pièce de la même couleur
    }

    private Piece? GetPieceFromBoard(Position pos)
    {
        return this.Board[pos.X, pos.Y];
    }

    private void SwitchTurn()
    {
        this.currentPLayer = this.currentPLayer == this.Players[0]
                                                  ? this.Players[1]
                                                  : this.Players[0];
    }

    private bool GameIsOver()
    {
        return false;
    }

    private void InitPlayers()
    {
        this.Players[0] = new Player { Color = Color.White, Name = "RecroX" };
        this.Players[1] = new Player { Color = Color.Black, Name = "BAAAAADNICK" };
    }

    private void InitChessBoard()
    {
        // Initialize white pieces
        Board[0, 0] = new Rook(new Position(0, 0), Color.White); // A1
        Board[0, 1] = new Knight(new Position(0, 1), Color.White); // B1
        Board[0, 2] = new Bishop(new Position(0, 2), Color.White); // C1
        Board[0, 3] = new Queen(new Position(0, 3), Color.White); // D1
        Board[0, 4] = new King(new Position(0, 4), Color.White); // E1
        Board[0, 5] = new Bishop(new Position(0, 5), Color.White); // F1
        Board[0, 6] = new Knight(new Position(0, 6), Color.White); // G1
        Board[0, 7] = new Rook(new Position(0, 7), Color.White); // H1
        for (int i = 0; i < 8; i++)
        {
            Board[1, i] = new Pawn(new Position(1, i), Color.White); // Ligne 2
        }

        // Initialize black pieces
        Board[7, 0] = new Rook(new Position(7, 0), Color.Black); // A8
        Board[7, 1] = new Knight(new Position(7, 1), Color.Black); // B8
        Board[7, 2] = new Bishop(new Position(7, 2), Color.Black); // C8
        Board[7, 3] = new Queen(new Position(7, 3), Color.Black); // D8
        Board[7, 4] = new King(new Position(7, 4), Color.Black); // E8
        Board[7, 5] = new Bishop(new Position(7, 5), Color.Black); // F8
        Board[7, 6] = new Knight(new Position(7, 6), Color.Black); // G8
        Board[7, 7] = new Rook(new Position(7, 7), Color.Black); // H8
        for (int i = 0; i < 8; i++)
        {
            Board[6, i] = new Pawn(new Position(6, i), Color.Black); // Ligne 7
        }
    }

    private void ShowChessBoard()
    {
        Console.WriteLine(this.ToString());
    }

    private bool IsPieceOnTrajectory(Position from, Position to)
    {
        // Obtenir les différences entre les coordonnées
        int deltaX = to.X - from.X;
        int deltaY = to.Y - from.Y;

        // Déterminer la direction du mouvement
        int stepX = deltaX == 0 ? 0 : deltaX / Math.Abs(deltaX); // 0, 1 ou -1
        int stepY = deltaY == 0 ? 0 : deltaY / Math.Abs(deltaY); // 0, 1 ou -1

        // Parcourir la trajectoire
        int currentX = from.X + stepX;
        int currentY = from.Y + stepY;

        while (currentX != to.X || currentY != to.Y)
        {
            if (this.Board[currentX, currentY] != null)
            {
                // Une pièce se trouve sur la trajectoire
                return true;
            }

            // Passer à la prochaine case sur la trajectoire
            currentX += stepX;
            currentY += stepY;
        }

        // Aucune pièce sur la trajectoire
        return false;
    }

    public override string ToString()
    {
        var strToReturn = string.Empty;

        var strPlayer = $"au tour de {this.currentPLayer.Name} de couleur {this.currentPLayer.Color.Name} \n\n";
        strToReturn += strPlayer;

        // Ajouter les lettres des colonnes (A à H)
        strToReturn += "   "; // Espace pour l'alignement des lettres des colonnes
        for (int col = 0; col < Board.GetLength(1); col++)
        {
            strToReturn += $"{(char)('A' + col)} ";
        }
        strToReturn += "\n";

        for (int row = 0; row < Board.GetLength(0); row++)
        {
            // Ajouter le numéro de la ligne (8 à 1)
            strToReturn += (8 - row) + "  "; // Espace pour l'alignement
            for (int col = 0; col < Board.GetLength(1); col++)
            {
                var piece = Board[row, col];
                strToReturn += piece != null ? piece.ToString() : ".";
                strToReturn += " ";
            }
            strToReturn += "\n";
        }

        return strToReturn + "\n";
    }
}
