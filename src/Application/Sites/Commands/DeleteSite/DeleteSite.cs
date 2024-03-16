using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Sites.Commands.DeleteSite
{
    [Authorize]
    public record DeleteSiteCommand : IRequest<Unit>
    {
        public string Id { get; init; }
    }

    public class DeleteSiteCommandValidator : AbstractValidator<DeleteSiteCommand>
    {
        public DeleteSiteCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class DeleteSiteCommandHandler : IRequestHandler<DeleteSiteCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSiteCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSiteCommand request, CancellationToken cancellationToken)
        {
            var site = await _context.Sites.FindAsync(request.Id);

            if (site == null)
            {
                throw new NotFoundException(nameof(Site), request.Id);
            }

            _context.Sites.Remove(site);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
