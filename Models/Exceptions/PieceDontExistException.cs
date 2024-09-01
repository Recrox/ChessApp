namespace ChessApp.Models.Exceptions;

[Serializable]
public class PieceDontExistException : Exception
{
    public PieceDontExistException()
    {
    }

    public PieceDontExistException(string? message) : base(message)
    {
    }

    public PieceDontExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}