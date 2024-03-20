using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Application.Sites.Queries.GetSite;
using VehiGate.Domain.Entities;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Application.Sites.Queries.GetSites
{
    [Authorize]
    public record GetSitesQuery : IRequest<PagedResult<SiteDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string SearchBy { get; init; }
        public string OrderBy { get; init; }
        public int SortOrder { get; init; } = 1;
    }

    public class GetSitesQueryHandler : IRequestHandler<GetSitesQuery, PagedResult<SiteDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSitesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<SiteDto>> Handle(GetSitesQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.Sites.AsNoTracking().ToListAsync();

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                query = query.Where(site =>
                    site.Address.Contains(request.SearchBy) ||
                    site.Contact.Contains(request.SearchBy) ||
                    site.Phone.Contains(request.SearchBy) ||
                    site.Email.Contains(request.SearchBy)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                bool sortOrder = request.SortOrder < 0 ? false : true;
                query = query.OrderByProperty(request.OrderBy, ascending: sortOrder).ToList();
            }

            var totalCount = query.Count();

            var sites = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            List<SiteDto> sitesDto = new List<SiteDto>();

            foreach (var site in sites)
            {
                sitesDto.Add(new SiteDto { Address = site.Address, Contact = site.Contact, Email = site.Email, Id = site.Id, Phone = site.Phone });
            }

            return PagedResult<SiteDto>.Create(sitesDto, request.PageNumber, request.PageSize);
        }
    }
}
