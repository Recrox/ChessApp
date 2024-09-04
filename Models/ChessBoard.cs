using ChessApp.Models.Exceptions;
using ChessApp.Models.Pieces;
using ChessApp.Models.Pieces.Concretes;
using System.Drawing;

namespace Models;

public class ChessBoard
{
    public const int COLUMN_SIZE = 8;
    public const int ROW_SIZE = 8;

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
        //InitChessBoard();
        InitChessBoardAlgebric();
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

    private Position AskFromPos()
    {
        // Demander la position de départ
        Console.WriteLine("Entrez la position de départ (format: LettreChiffre) :");
        var fromInput = Console.ReadLine();
        var from = new Position(fromInput); // Utiliser le constructeur avec une chaîne
        return from;
    }

    private Position AskToPos()
    {
        // Demander la position d'arrivée
        Console.WriteLine("Entrez la position d'arrivée (format: LettreChiffre) :");
        var toInput = Console.ReadLine();
        var to = new Position(toInput); // Utiliser le constructeur avec une chaîne
        return to;
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
            EatPiece(pieceToMove, pieceAtTarget);

        // Déplacement
        this.Board[to.X, to.Y] = pieceToMove;
        this.Board[from.X, from.Y] = null;
        pieceToMove.Position = to; // Met à jour la position de la pièce qui se déplace
    }

    private void EatPiece(Piece pieceToMove, Piece pieceAtTarget)
    {
        Console.WriteLine($"Pièce à la position d'arrivée : {pieceAtTarget.ToString()} à la position {pieceAtTarget.Position}");
        if (pieceAtTarget.Color == pieceToMove.Color)
            throw new SameColorException(); // Ne peut pas se déplacer sur une pièce de la même couleur

        this.Board[pieceAtTarget.Position.X, pieceAtTarget.Position.Y] = null;
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

    private void InitChessBoardAlgebric()
    {
        // Initialize white pieces
        Board[0, 0] = new Rook(new Position("A1"), Color.White);
        Board[0, 1] = new Knight(new Position("B1"), Color.White);
        Board[0, 2] = new Bishop(new Position("C1"), Color.White);
        Board[0, 3] = new Queen(new Position("D1"), Color.White);
        Board[0, 4] = new King(new Position("E1"), Color.White);
        Board[0, 5] = new Bishop(new Position("F1"), Color.White);
        Board[0, 6] = new Knight(new Position("G1"), Color.White);
        Board[0, 7] = new Rook(new Position("H1"), Color.White);

        // Place white pawns on the second row
        for (int i = 0; i < 8; i++)
        {
            Board[1, i] = new Pawn(new Position($"{(char)('A' + i)}2"), Color.White);
        }

        // Initialize black pieces
        Board[7, 0] = new Rook(new Position("A8"), Color.Black);
        Board[7, 1] = new Knight(new Position("B8"), Color.Black);
        Board[7, 2] = new Bishop(new Position("C8"), Color.Black);
        Board[7, 3] = new Queen(new Position("D8"), Color.Black);
        Board[7, 4] = new King(new Position("E8"), Color.Black);
        Board[7, 5] = new Bishop(new Position("F8"), Color.Black);
        Board[7, 6] = new Knight(new Position("G8"), Color.Black);
        Board[7, 7] = new Rook(new Position("H8"), Color.Black);

        // Place black pawns on the seventh row
        for (int i = 0; i < 8; i++)
        {
            Board[6, i] = new Pawn(new Position($"{(char)('A' + i)}7"), Color.Black);
        }
    }




    private void ShowChessBoard()
    {
        Console.WriteLine(this.ToString());
    }

    private bool IsPieceOnTrajectory(Position from, Position to)
    {
        // Obtenir les différences entre les coordonnées
        Position delta = to - from;

        // Déterminer la direction du mouvement
        int stepX = delta.X == 0 ? 0 : delta.X / Math.Abs(delta.X); // 0, 1 ou -1
        int stepY = delta.Y == 0 ? 0 : delta.Y / Math.Abs(delta.Y); // 0, 1 ou -1

        // Créer une position intermédiaire
        Position currentPos = from + new Position(stepX, stepY);

        while (!currentPos.Equals(to))
        {
            if (this.Board[currentPos.X, currentPos.Y] != null)
            {
                // Une pièce se trouve sur la trajectoire
                return true;
            }

            // Passer à la prochaine case sur la trajectoire
            currentPos = currentPos + new Position(stepX, stepY);
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

        // Ajouter les lignes du plateau (8 à 1)
        for (int row = Board.GetLength(0) - 1; row >= 0; row--)
        {
            // Ajouter le numéro de la ligne (8 à 1)
            strToReturn += (row + 1) + "  "; // Espace pour l'alignement
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
