using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Teams.Commands
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, bool>
    {
        private readonly AppDBContext _context;

        public DeleteTeamCommandHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (team == null)
                return false;

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
