namespace SnakesAndLadders.Domain.Entities;

public class GamePlayer : AuditableEntity
{
    public int GameId { get; set; }

    public int PlayerId { get; set; }

    public int CurrentBoardSquareId { get; set; }

    public int TurnPosition { get; set; }

    public bool IsTheirTurn { get; set; }

    public bool IsWinner { get; set; }

    public Game Game { get; set; } = null!;

    public Player Player { get; set; } = null!;

    public BoardSquare CurrentBoardSquare { get; set; } = null!;
}
