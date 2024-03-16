using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Application.Sites.Commands.CreateSite;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Sites.Commands.CreateSite
{
    [Authorize]
    public record CreateSiteCommand : IRequest<string>
    {
        public string Address { get; init; }
        public string Contact { get; init; }
        public string Phone { get; init; }
        public string Email { get; init; }
    }

    public class CreateSiteCommandValidator : AbstractValidator<CreateSiteCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateSiteCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            var existingSite = await _context.Sites.FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
            return existingSite == null;
        }
    }
}

public class CreateSiteCommandHandler : IRequestHandler<CreateSiteCommand, string>
{
    private readonly IApplicationDbContext _context;

    public CreateSiteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
    {
        var site = new Site
        {
            Address = request.Address,
            Contact = request.Contact,
            Phone = request.Phone,
            Email = request.Email
        };

        _context.Sites.Add(site);
        await _context.SaveChangesAsync(cancellationToken);

        return site.Id;
    }
}
