using MediatR;
using Microsoft.EntityFrameworkCore;
using SnakesAndLadders.Application.Common.Interfaces;
using SnakesAndLadders.Domain.Common;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Application.Games.Commands.CreateGame;

public class AddPlayerToGameCommand : IRequest<Unit>
{
    public int GameId { get; set; }

    public int PlayerId { get; set; }
}

public class AddPlayerToGameCommandHandler : IRequestHandler<AddPlayerToGameCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AddPlayerToGameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddPlayerToGameCommand request, CancellationToken cancellationToken)
    {
        Game game = await _context.Games.Include(g => g.Board).ThenInclude(b=> b.BoardSquares).SingleAsync(g => g.Id.Equals(request.GameId));
        int actualGamePlayers = game.GamePlayers.Count;
        GamePlayer entity = new GamePlayer
        {
            GameId = request.GameId,
            PlayerId = request.PlayerId,
            CurrentBoardSquare = game.Board.BoardSquares.First(bs => bs.PositionNumber.Equals(Constants.BOARD_MIN_POSITION)),
            TurnPosition = actualGamePlayers + 1,
            IsTheirTurn = actualGamePlayers.Equals(0),
            IsWinner = false
        };

        _context.GamePlayers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
