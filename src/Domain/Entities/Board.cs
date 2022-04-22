namespace SnakesAndLadders.Domain.Entities;

public class Board : AuditableEntity
{
    public int Id { get; set; }

    public IList<BoardSquare> BoardSquares { get; private set; } = new List<BoardSquare>();

    public IList<Game> Games { get; private set; } = new List<Game>();
}
