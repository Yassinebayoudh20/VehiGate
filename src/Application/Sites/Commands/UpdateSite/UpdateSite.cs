using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Sites.Commands.UpdateSite
{
    [Authorize]
    public record UpdateSiteCommand : IRequest<Unit>
    {
        public string Id { get; init; }
        public string Address { get; init; }
        public string Contact { get; init; }
        public string Phone { get; init; }
        public string Email { get; init; }
    }

    public class UpdateSiteCommandValidator : AbstractValidator<UpdateSiteCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateSiteCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address.")
                .MustAsync((command, email, cancellationToken) => BeUniqueEmail(command.Id, email, cancellationToken))
                .WithMessage("Email must be unique.");
        }

        private async Task<bool> BeUniqueEmail(string id, string email, CancellationToken cancellationToken)
        {
            var existingSite = await _context.Sites.FirstOrDefaultAsync(s => s.Id != id && s.Email == email, cancellationToken);
            return existingSite == null;
        }
    }

    public class UpdateSiteCommandHandler : IRequestHandler<UpdateSiteCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateSiteCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateSiteCommand request, CancellationToken cancellationToken)
        {
            var site = await _context.Sites.FindAsync(request.Id);

            if (site == null)
            {
                throw new NotFoundException(nameof(Site), request.Id);
            }

            if (request.Address != null)
            {
                site.Address = request.Address;
            }

            if (request.Contact != null)
            {
                site.Contact = request.Contact;
            }

            if (request.Phone != null)
            {
                site.Phone = request.Phone;
            }

            if (request.Email != null)
            {
                site.Email = request.Email;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
