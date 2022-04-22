using FluentAssertions;
using NUnit.Framework;
using SnakesAndLadders.Application.Games.Commands.CreateGame;
using SnakesAndLadders.Application.Players.Commands.CreatePlayer;
using SnakesAndLadders.Application.Players.Queries.GetPlayerCurrentSituation;
using SnakesAndLadders.Domain.Common;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Application.IntegrationTests.Games.Commands;

using static Testing;

public class RollDiceTests : TestBase
{
    #region US 1 - Token Can Move Across the Board

    [Test]
    public async Task TokenCanMoveAcrossTheBoard()
    {
        Board board = await FindAsync<Board>(1);

        var gameId = await SendAsync(new CreateGameCommand
        {
            BoardId = board.Id
        });

        var playerId = await SendAsync(new CreatePlayerCommand
        {
            Nickname = "Newuser1"
        });

        await SendAsync(new AddPlayerToGameCommand
        {
            GameId = gameId,
            PlayerId = playerId
        });

        await SendAsync(new StartGameCommand
        {
            GameId = gameId
        });

        // UAT1

        GetPlayerCurrentSituationQueryDto playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.HasGameFinished.Should().Be(false);
        playerCurrentSituation.PlayerCurrentPosition.Should().Be(1);
        playerCurrentSituation.IsPlayersTurn.Should().Be(true);
        playerCurrentSituation.IsPlayerWinner.Should().Be(false);

        // UAT2

        await SendAsync(new RollDiceCommand
        {
            GameId = gameId,
            RollDiceResult = 3
        });

        playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.HasGameFinished.Should().Be(false);
        playerCurrentSituation.PlayerCurrentPosition.Should().Be(4);
        playerCurrentSituation.IsPlayersTurn.Should().Be(true);
        playerCurrentSituation.IsPlayerWinner.Should().Be(false);

        // UAT3

        await SendAsync(new RollDiceCommand
        {
            GameId = gameId,
            RollDiceResult = 4
        });

        playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.HasGameFinished.Should().Be(false);
        playerCurrentSituation.PlayerCurrentPosition.Should().Be(8);
        playerCurrentSituation.IsPlayersTurn.Should().Be(true);
        playerCurrentSituation.IsPlayerWinner.Should().Be(false);
    }

    #endregion US 1 - Token Can Move Across the Board

    #region US 2 - Player Can Win the Game

    // UAT1
    [Test]
    public async Task PlayerShouldWinTheGame()
    {
        Board board = await FindAsync<Board>(1);

        var gameId = await SendAsync(new CreateGameCommand
        {
            BoardId = board.Id
        });

        var playerId = await SendAsync(new CreatePlayerCommand
        {
            Nickname = "Newuser1"
        });

        await SendAsync(new AddPlayerToGameCommand
        {
            GameId = gameId,
            PlayerId = playerId
        });

        await SendAsync(new StartGameCommand
        {
            GameId = gameId
        });

        for(int i= 0; i < 16; i++)
        {
            await SendAsync(new RollDiceCommand
            {
                GameId = gameId,
                RollDiceResult = 6
            });
        }

        GetPlayerCurrentSituationQueryDto playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.HasGameFinished.Should().Be(false);
        playerCurrentSituation.PlayerCurrentPosition.Should().Be(97);
        playerCurrentSituation.IsPlayersTurn.Should().Be(true);
        playerCurrentSituation.IsPlayerWinner.Should().Be(false);

        await SendAsync(new RollDiceCommand
        {
            GameId = gameId,
            RollDiceResult = 3
        });

        playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.HasGameFinished.Should().Be(true);
        playerCurrentSituation.PlayerCurrentPosition.Should().Be(Constants.BOARD_MAX_POSITION);
        playerCurrentSituation.IsPlayerWinner.Should().Be(true);
    }

    // UAT2
    [Test]
    public async Task PlayerShouldNotWinTheGame()
    {
        Board board = await FindAsync<Board>(1);

        var gameId = await SendAsync(new CreateGameCommand
        {
            BoardId = board.Id
        });

        var playerId = await SendAsync(new CreatePlayerCommand
        {
            Nickname = "Newuser1"
        });

        await SendAsync(new AddPlayerToGameCommand
        {
            GameId = gameId,
            PlayerId = playerId
        });

        await SendAsync(new StartGameCommand
        {
            GameId = gameId
        });

        for (int i = 0; i < 16; i++)
        {
            await SendAsync(new RollDiceCommand
            {
                GameId = gameId,
                RollDiceResult = 6
            });
        }

        GetPlayerCurrentSituationQueryDto playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.HasGameFinished.Should().Be(false);
        playerCurrentSituation.PlayerCurrentPosition.Should().Be(97);
        playerCurrentSituation.IsPlayersTurn.Should().Be(true);
        playerCurrentSituation.IsPlayerWinner.Should().Be(false);

        await SendAsync(new RollDiceCommand
        {
            GameId = gameId,
            RollDiceResult = 4
        });

        playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.HasGameFinished.Should().Be(false);
        playerCurrentSituation.PlayerCurrentPosition.Should().Be(97);
        playerCurrentSituation.IsPlayerWinner.Should().Be(false);
    }

    #endregion US 2 - Player Can Win the Game

    #region US 3 - Moves Are Determined By Dice Rolls

    // UAT1
    [Test]
    public async Task RollDiceResultMustBeBetweenExpectedValues()
    {
        Board board = await FindAsync<Board>(1);

        var gameId = await SendAsync(new CreateGameCommand
        {
            BoardId = board.Id
        });

        var playerId = await SendAsync(new CreatePlayerCommand
        {
            Nickname = "Newuser1"
        });

        await SendAsync(new AddPlayerToGameCommand
        {
            GameId = gameId,
            PlayerId = playerId
        });

        await SendAsync(new StartGameCommand
        {
            GameId = gameId
        });

        await SendAsync(new RollDiceCommand
        {
            GameId = gameId
        });
        
        GetPlayerCurrentSituationQueryDto playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.PlayerCurrentPosition.Should().BeGreaterThan(1);
        playerCurrentSituation.PlayerCurrentPosition.Should().BeLessThan(8);
    }

    // UAT2
    [Test]
    public async Task TokenMoveIsDeterminedByRollDice()
    {
        Board board = await FindAsync<Board>(1);

        var gameId = await SendAsync(new CreateGameCommand
        {
            BoardId = board.Id
        });

        var playerId = await SendAsync(new CreatePlayerCommand
        {
            Nickname = "Newuser1"
        });

        await SendAsync(new AddPlayerToGameCommand
        {
            GameId = gameId,
            PlayerId = playerId
        });

        await SendAsync(new StartGameCommand
        {
            GameId = gameId
        });

        await SendAsync(new RollDiceCommand
        {
            GameId = gameId,
            RollDiceResult = 4
        });

        GetPlayerCurrentSituationQueryDto playerCurrentSituation = await SendAsync(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = playerId
        });

        playerCurrentSituation.Should().NotBeNull();
        playerCurrentSituation.HasGameStarted.Should().Be(true);
        playerCurrentSituation.PlayerCurrentPosition.Should().Be(5);
    }

    #endregion US 3 - Moves Are Determined By Dice Rolls
}
