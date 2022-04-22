using MediatR;
using Microsoft.EntityFrameworkCore;
using SnakesAndLadders.Application.Common.Interfaces;
using SnakesAndLadders.Domain.Common;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Application.Games.Commands.CreateGame;

public class RollDiceCommand : IRequest<Unit>
{
    public int GameId { get; set; }

    public int? RollDiceResult { get; set; }
}

public class RollDiceCommandHandler : IRequestHandler<RollDiceCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RollDiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    private int GetRollDiceResult(int? inputResult)
    {
        int rollDiceResult;

        if (inputResult.HasValue)
        {
            rollDiceResult = inputResult.Value;
        }
        else
        {
            Random random = new();
            rollDiceResult = random.Next(Constants.ROLL_DICE_MIN_VALUE, Constants.ROLL_DICE_MAX_VALUE);
        }

        return rollDiceResult;
    }

    public async Task<Unit> Handle(RollDiceCommand request, CancellationToken cancellationToken)
    {
        Game game = await _context.Games.FindAsync(request.GameId);
        GamePlayer currentGamePlayer = await _context.GamePlayers.Include(gp => gp.CurrentBoardSquare).FirstAsync(gp => gp.GameId.Equals(request.GameId) && gp.IsTheirTurn);
        currentGamePlayer.IsTheirTurn = false;
        int rollDiceResult = GetRollDiceResult(request.RollDiceResult);
        int newPositionNumber = currentGamePlayer.CurrentBoardSquare.PositionNumber + rollDiceResult;

        if (newPositionNumber > Constants.BOARD_MAX_POSITION)
        {
            // If the new position number is higher than the maximum position number, the player's position does not change
            newPositionNumber = currentGamePlayer.CurrentBoardSquare.PositionNumber;
        }
        else if (newPositionNumber < Constants.BOARD_MAX_POSITION)
        {
            // We have to handle snakes and ladders
            BoardSquare newBoardSquare = await _context.BoardSquares.FirstAsync(bs => bs.Board.Games.Select(g => g.Id).Contains(request.GameId) && bs.PositionNumber.Equals(newPositionNumber));

            if (newBoardSquare.BoardSquareToBeMoved != null)
            {
                newBoardSquare = newBoardSquare.BoardSquareToBeMoved;
            }

            currentGamePlayer.CurrentBoardSquare = newBoardSquare;

            // Set the current turn to the next player
            int newTurnPosition = currentGamePlayer.TurnPosition + 1;

            if (newTurnPosition > game.GamePlayers.Count)
            {
                newTurnPosition = 1;
            }

            GamePlayer nextGamePlayer = await _context.GamePlayers.FirstAsync(gp => gp.GameId.Equals(request.GameId) && gp.TurnPosition.Equals(newTurnPosition));
            nextGamePlayer.IsTheirTurn = true;
        }
        else
        {
            // The player has reached the last square: they win the game
            BoardSquare newBoardSquare = await _context.BoardSquares.FirstAsync(bs => bs.Board.Games.Select(g => g.Id).Contains(request.GameId) && bs.PositionNumber.Equals(Constants.BOARD_MAX_POSITION));
            currentGamePlayer.CurrentBoardSquare = newBoardSquare;
            currentGamePlayer.IsWinner = true;
            game.HasFinished = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
