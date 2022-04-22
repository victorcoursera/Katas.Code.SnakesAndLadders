using MediatR;
using SnakesAndLadders.Application.Common.Interfaces;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Application.Players.Commands.CreatePlayer;

public class CreatePlayerCommand : IRequest<int>
{
    public string? Nickname { get; set; }
}

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Player
        {
           Nickname = request.Nickname
        };

        _context.Players.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
