using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnakesAndLadders.Application.Common.Interfaces;

namespace SnakesAndLadders.Application.Players.Queries.GetPlayerCurrentSituation;

public class GetPlayerCurrentSituationQuery : IRequest<GetPlayerCurrentSituationQueryDto>
{
    public int GameId { get; set; }
    public int PlayerId { get; set; }
}

public class GetPlayerCurrentSituationQueryHandler : IRequestHandler<GetPlayerCurrentSituationQuery, GetPlayerCurrentSituationQueryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlayerCurrentSituationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetPlayerCurrentSituationQueryDto> Handle(GetPlayerCurrentSituationQuery request, CancellationToken cancellationToken)
    {
        return await _context.GamePlayers
            .Where(gp => gp.GameId.Equals(request.GameId) && gp.PlayerId.Equals(request.PlayerId))
            .ProjectTo<GetPlayerCurrentSituationQueryDto>(_mapper.ConfigurationProvider)
            .FirstAsync();
    }
}
