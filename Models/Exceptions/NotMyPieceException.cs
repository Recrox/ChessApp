namespace ChessApp.Models.Exceptions;

[Serializable]
internal class NotMyPieceException : CantMoveException
{
    public NotMyPieceException()
    {
    }

    public NotMyPieceException(string? message) : base(message)
    {
    }

    public NotMyPieceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}