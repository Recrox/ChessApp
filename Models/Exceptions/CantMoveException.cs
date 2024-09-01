namespace ChessApp.Models.Exceptions;

[Serializable]
public class CantMoveException : Exception
{
    public CantMoveException()
    {
    }

    public CantMoveException(string? message) : base(message)
    {
    }

    public CantMoveException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
