using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartShipping.Application.Features.Shipments.DTOs;
using SmartShipping.Application.Features.Shipments.Queries;
using SmartShipping.Application.Interfaces;
using SmartShipping.Shared.Services;

public class GetShipmentByIdQueryHandler : IRequestHandler<GetShipmentByIdQuery, ShipmentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly RedisCacheService _cache;

    public GetShipmentByIdQueryHandler(IApplicationDbContext context, RedisCacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<ShipmentDto> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"shipment:{request.Id}";
        var cached = await _cache.GetAsync<ShipmentDto>(cacheKey);
        if (cached != null)
            return cached;

        var shipment = await _context.Shipments
            .Include(x => x.AssignedCarrierCompany)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shipment == null)
            throw new Exception("Shipment not found");

        var dto = new ShipmentDto
        {
            Id = shipment.Id,
            OrderId = shipment.OrderId,
            City = shipment.City,
            Weight = shipment.Weight,
            PreferredDeliveryTimeInHours = shipment.PreferredDeliveryTimeInHours,
            CarrierCompanyName = shipment.AssignedCarrierCompany?.Name
        };

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return dto;
    }

}
