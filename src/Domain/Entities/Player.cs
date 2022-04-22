namespace SnakesAndLadders.Domain.Entities;

public class Player : AuditableEntity
{
    public int Id { get; set; }

    public string? Nickname { get; set; }

    public IList<GamePlayer> GamePlayers { get; private set; } = new List<GamePlayer>();
}
