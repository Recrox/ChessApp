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

    //public ChessBoard(Player[] players) : this()
    //{
        
    //}

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
                // Demander la position de départ
                Console.WriteLine("Entrez la position de départ (format: x y) :");
                var fromInput = Console.ReadLine();
                var from = ParsePosition(fromInput);

                // Demander la position d'arrivée
                Console.WriteLine("Entrez la position d'arrivée (format: x y) :");
                var toInput = Console.ReadLine();
                var to = ParsePosition(toInput);

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
                Console.WriteLine("Format de position invalide. Assurez-vous d'utiliser le format 'x y'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
            }
        }
    }

    private Position ParsePosition(string input)
    {
        var parts = input.Split(' ');
        if (parts.Length != 2)
            throw new FormatException("La position doit être au format 'x y'.");

        if (!int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
            throw new FormatException("Les coordonnées doivent être des nombres entiers.");

        if (x < 0 || x >= 8 || y < 0 || y >= 8)
            throw new ArgumentOutOfRangeException("Les coordonnées doivent être entre 0 et 7 compris.");

        return new Position(x, y);
    }


    private void MovePiece(Position from, Position to)
    {
        //Check avant Deplacement
        var pieceToMove = this.GetPieceFromBoard(from) ?? throw new PieceDontExistException();
        Console.WriteLine($"Pièce sélectionnée : {pieceToMove.ToString()} à la position ({from.X}, {from.Y})");

        if(pieceToMove.Color != this.currentPLayer.Color)
            throw new NotMyPieceException();

        if (!pieceToMove.IsMovable(to))
            throw new CantMoveException();

        Piece? pieceAtTarget = GetPieceFromBoard(to);
        //si c'est une prise de piece.
        if (pieceAtTarget is not null)
            EatPiece(to, pieceToMove, pieceAtTarget);

        // Déplacement
        this.Board[to.X, to.Y] = pieceToMove;
        this.Board[from.X, from.Y] = null;
        pieceToMove.Position = to; // Met à jour la position de la pièce
    }

    private void EatPiece(Position to, Piece pieceToMove, Piece pieceAtTarget)
    {
        Console.WriteLine($"Pièce à la position d'arrivée : {pieceAtTarget.ToString()} à la position ({to.X}, {to.Y})");
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
                                                   ?this.Players[1] 
                                                   :this.Players[0];
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
        Board[0, 0] = new Rook(new Position(0, 0), Color.White);
        Board[0, 1] = new Knight(new Position(0, 1), Color.White);
        Board[0, 2] = new Bishop(new Position(0, 2), Color.White);
        Board[0, 3] = new Queen(new Position(0, 3), Color.White);
        Board[0, 4] = new King(new Position(0, 4), Color.White);
        Board[0, 5] = new Bishop(new Position(0, 5), Color.White);
        Board[0, 6] = new Knight(new Position(0, 6), Color.White);
        Board[0, 7] = new Rook(new Position(0, 7), Color.White);
        for (int i = 0; i < 8; i++)
        {
            Board[1, i] = new Pawn(new Position(1, i), Color.White);
        }

        // Initialize black pieces
        Board[7, 0] = new Rook(new Position(7, 0), Color.Black);
        Board[7, 1] = new Knight(new Position(7, 1), Color.Black);
        Board[7, 2] = new Bishop(new Position(7, 2), Color.Black);
        Board[7, 3] = new Queen(new Position(7, 3), Color.Black);
        Board[7, 4] = new King(new Position(7, 4), Color.Black);
        Board[7, 5] = new Bishop(new Position(7, 5), Color.Black);
        Board[7, 6] = new Knight(new Position(7, 6), Color.Black);
        Board[7, 7] = new Rook(new Position(7, 7), Color.Black);
        for (int i = 0; i < 8; i++)
        {
            Board[6, i] = new Pawn(new Position(6, i), Color.Black);
        }
    }

    private void ShowChessBoard()
    {
        Console.WriteLine(this.ToString());
    }

    public override string ToString()
    {
        var strToReturn = string.Empty;

        var strPlayer = $"au tour de {this.currentPLayer.ToString()} de couleur {this.currentPLayer.Color} \n\n";
        strToReturn += strPlayer;
        for (int row = 0; row < Board.GetLength(0); row++)
        {
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
