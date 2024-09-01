using ChessApp.Models.Pieces;
using System.Drawing;

namespace Models;

public class ChessBoard
{
    public Player[] Players { get; set; }
    private Piece[,] Board;

    public ChessBoard()
    {
        this.Board = new Piece[8, 8];
    }

    public void StartGame()
    {
        ShowChessBoard();

        InitChessBoard();

        ShowChessBoard();

    }

    private void InitChessBoard()
    {
        // Initialize white pieces
        Board[0, 0] = new Rook { Color = Color.White };
        Board[0, 1] = new Knight { Color = Color.White };
        Board[0, 2] = new Bishop { Color = Color.White };
        Board[0, 3] = new Queen { Color = Color.White };
        Board[0, 4] = new King { Color = Color.White };
        Board[0, 5] = new Bishop { Color = Color.White };
        Board[0, 6] = new Knight { Color = Color.White };
        Board[0, 7] = new Rook { Color = Color.White };
        for (int i = 0; i < 8; i++)
        {
            Board[1, i] = new Pawn { Color = Color.White };
        }

        // Initialize black pieces
        Board[7, 0] = new Rook { Color = Color.Black };
        Board[7, 1] = new Knight { Color = Color.Black };
        Board[7, 2] = new Bishop { Color = Color.Black };
        Board[7, 3] = new Queen { Color = Color.Black };
        Board[7, 4] = new King { Color = Color.Black };
        Board[7, 5] = new Bishop { Color = Color.Black };
        Board[7, 6] = new Knight { Color = Color.Black };
        Board[7, 7] = new Rook { Color = Color.Black };
        for (int i = 0; i < 8; i++)
        {
            Board[6, i] = new Pawn { Color = Color.Black };
        }
    }

    private void ShowChessBoard()
    {
        Console.WriteLine(this.ToString());
    }

    public override string ToString()
    {
        var strToReturn = string.Empty;

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
