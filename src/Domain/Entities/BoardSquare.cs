namespace SnakesAndLadders.Domain.Entities;

public class BoardSquare : AuditableEntity
{
    public int Id { get; set; }

    public int BoardId { get; set; }

    public int? BoardSquareToBeMovedId { get; set; }

    public int PositionNumber { get; set; }

    public Board Board { get; set; } = null!;

    public BoardSquare BoardSquareToBeMoved { get; set; } = null!;
}
