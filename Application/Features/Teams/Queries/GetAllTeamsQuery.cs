using Domain.DTOs;
using MediatR;

namespace Application.Features.Teams.Queries
{
    public class GetAllTeamsQuery : IRequest<List<TeamDTO>>
    {
    }
}
