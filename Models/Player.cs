using System.Drawing;

namespace Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Color Color { get; set; }
    public Position? PiecePositionChoice  { get; set; }

    public override string ToString()
    {
        return this.Name ?? "undefined";
    }

}