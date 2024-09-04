using System.Drawing;

namespace ChessApp.Utils;

public static class ColorExtensions
{
    public static string ToCustomString(this Color color)
    {
        return color == Color.White ? "Blanc" :
               color == Color.Black ? "Noir" :
               $"Couleur personnalisée (R: {color.R}, G: {color.G}, B: {color.B})";
    }
}