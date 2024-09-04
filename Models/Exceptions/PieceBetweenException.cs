
namespace ChessApp.Models.Exceptions;

public class PieceBetweenException : CantMoveException
{
    public PieceBetweenException()
    {
    }

    public PieceBetweenException(string? message) : base(message)
    {
    }

    public PieceBetweenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
