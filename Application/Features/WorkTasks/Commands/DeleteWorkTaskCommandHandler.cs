using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkTasks.Commands
{
    public class DeleteWorkTaskCommandHandler : IRequestHandler<DeleteWorkTaskCommand, bool>
    {
        private readonly AppDBContext _context;

        public DeleteWorkTaskCommandHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteWorkTaskCommand request, CancellationToken cancellationToken)
        {
            var workTask = await _context.WorkTasks
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (workTask == null)
                return false;

            _context.WorkTasks.Remove(workTask);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
