using MediatR;
using SnakesAndLadders.Application.Common.Interfaces;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Application.Games.Commands.CreateGame;

public class CreateGameCommand : IRequest<int>
{
    public int BoardId { get; set; }
}

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateGameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var entity = new Game
        {
           BoardId = request.BoardId
        };

        _context.Games.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
