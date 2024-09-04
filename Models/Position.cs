namespace Models;
public class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Constructeur pour les coordonnées algébriques
    public Position(string algebraic)
    {
        if (IsGoodSize(algebraic))
            throw new FormatException("La position doit être au format 'LettreChiffre' (ex: A1, B2).");

        char columnChar = char.ToUpper(algebraic[0]);
        if (!char.IsLetter(columnChar) || !int.TryParse(algebraic.Substring(1), out int row))
            throw new FormatException("Format de position invalide. Assurez-vous d'utiliser le format 'LettreChiffre'.");

        // Convertir la lettre de colonne (A-H) en indice de colonne (0-7)
        X = columnChar - 'A'; // 'A' devient 0, 'B' devient 1, ..., 'H' devient 7

        // Convertir le numéro de ligne (1-8) en indice de ligne (7-0)
        Y = 8 - row; // 1 devient 7, 2 devient 6, ..., 8 devient 0

        if (IsOutChessBoard())
            throw new ArgumentOutOfRangeException("Les coordonnées doivent être dans la plage de l'échiquier.");
    }

    private bool IsGoodSize(string algebraic)
    {
        return string.IsNullOrWhiteSpace(algebraic) || algebraic.Length < 2;
    }

    private bool IsOutChessBoard()
    {
        return X < 0 || X >= 8 || Y < 0 || Y >= 8;
    }

    public static Position operator +(Position pos1, Position pos2)
    {
        return new Position(pos1.X + pos2.X, pos1.Y + pos2.Y);
    }

    // Surcharge de l'opérateur -
    public static Position operator -(Position pos1, Position pos2)
    {
        return new Position(pos1.X - pos2.X, pos1.Y - pos2.Y);
    }

    public override bool Equals(object? obj)
    {
        return obj is Position other 
            ? X == other.X && Y == other.Y 
            : false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        char columnChar = (char)('A' + X); // Convertir l'indice de colonne en lettre (0 devient 'A', 1 devient 'B', etc.)
        int rowNumber = 8 - Y; // Convertir l'indice de ligne en numéro de ligne (0 devient 8, 1 devient 7, etc.)

        return $"{columnChar}{rowNumber}";
    }
}
