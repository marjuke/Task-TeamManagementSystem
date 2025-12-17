using Domain.DTOs;
using MediatR;

namespace Application.Features.Teams.Queries
{
    public class GetTeamByIdQuery : IRequest<TeamDTO?>
    {
        public int Id { get; set; }

        public GetTeamByIdQuery(int id)
        {
            Id = id;
        }
    }
}
