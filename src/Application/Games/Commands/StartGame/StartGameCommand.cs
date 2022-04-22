using MediatR;
using SnakesAndLadders.Application.Common.Interfaces;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Application.Games.Commands.CreateGame;

public class StartGameCommand : IRequest<Unit>
{
    public int GameId { get; set; }
}

public class StartGameCommandHandler : IRequestHandler<StartGameCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public StartGameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        Game game = await _context.Games.FindAsync(request.GameId);
        game.HasStarted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
