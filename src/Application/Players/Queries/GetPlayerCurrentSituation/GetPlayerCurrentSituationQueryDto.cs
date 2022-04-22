using AutoMapper;
using SnakesAndLadders.Application.Common.Mappings;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Application.Players.Queries.GetPlayerCurrentSituation;

public class GetPlayerCurrentSituationQueryDto : IMapFrom<GamePlayer>
{
    public bool HasGameStarted { get; set; }

    public bool HasGameFinished { get; set; }

    public int PlayerCurrentPosition { get; set; }

    public bool IsPlayersTurn { get; set; }

    public bool IsPlayerWinner { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<GamePlayer, GetPlayerCurrentSituationQueryDto>()
            .ForMember(d => d.HasGameStarted, opt => opt.MapFrom(s => s.Game.HasStarted))
            .ForMember(d => d.HasGameFinished, opt => opt.MapFrom(s => s.Game.HasFinished))
            .ForMember(d => d.PlayerCurrentPosition, opt => opt.MapFrom(s => s.CurrentBoardSquare.PositionNumber))
            .ForMember(d => d.IsPlayersTurn, opt => opt.MapFrom(s => s.IsTheirTurn))
            .ForMember(d => d.IsPlayerWinner, opt => opt.MapFrom(s => s.IsWinner));
    }
}
