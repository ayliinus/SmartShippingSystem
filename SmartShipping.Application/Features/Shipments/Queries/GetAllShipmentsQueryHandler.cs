using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartShipping.Application.Features.Shipments.DTOs;
using SmartShipping.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartShipping.Application.Features.Shipments.Queries
{
    public class GetAllShipmentsQueryHandler
        : IRequestHandler<GetAllShipmentsQuery, List<ShipmentDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllShipmentsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShipmentDto>> Handle(GetAllShipmentsQuery request, CancellationToken cancellationToken)
        {
            var shipments = await _context.Shipments
                .Include(x => x.AssignedCarrierCompany)
                .ToListAsync(cancellationToken);

            return shipments.Select(s => new ShipmentDto
            {
                Id = s.Id,
                OrderId = s.OrderId,
                City = s.City,
                Weight = s.Weight,
                PreferredDeliveryTimeInHours = s.PreferredDeliveryTimeInHours,
                CarrierCompanyName = s.AssignedCarrierCompany?.Name
            }).ToList();
        }
    }
}
