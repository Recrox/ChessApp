// See https://aka.ms/new-console-template for more information
using Models;

Console.WriteLine("Voici un petit jeux d'echec en mode console :)");
Console.WriteLine("\n");

Console.OutputEncoding = System.Text.Encoding.UTF8;

var chessBoard = new ChessBoard();
chessBoard.StartGame();