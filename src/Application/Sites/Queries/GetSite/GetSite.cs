using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Sites.Queries.GetSite
{
    [Authorize]
    public record GetSiteQuery : IRequest<SiteDto>
    {
        public string Id { get; init; }
    }

    public class GetSiteQueryHandler : IRequestHandler<GetSiteQuery, SiteDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSiteQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SiteDto> Handle(GetSiteQuery request, CancellationToken cancellationToken)
        {
            var site = await _context.Sites
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (site == null)
            {
                throw new NotFoundException(nameof(Site), request.Id);
            }

            return new SiteDto { Address = site.Address, Contact = site.Contact, Email = site.Email, Id = site.Id, Phone = site.Phone };

        }
    }
}
