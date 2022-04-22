namespace SnakesAndLadders.Domain.Entities;

public class Game : AuditableEntity
{
    public int Id { get; set; }

    public int BoardId { get; set; }

    public bool HasStarted { get; set; }

    public bool HasFinished { get; set; }

    public Board Board { get; set; } = null!;

    public IList<GamePlayer> GamePlayers { get; private set; } = new List<GamePlayer>();
}
