using MediatR;
using SmartShipping.Domain.Entities;
using SmartShipping.Application.Interfaces;
using MassTransit;
using SmartShipping.Shared.Events;
using Microsoft.Extensions.Logging;

namespace SmartShipping.Application.Features.Shipments.Commands;

public class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICarrierSelectorService _selector;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateShipmentCommandHandler> _logger;
    public CreateShipmentCommandHandler(IApplicationDbContext context, ICarrierSelectorService selector, IPublishEndpoint publishEndpoint, ILogger<CreateShipmentCommandHandler> logger)
    {
        _context = context;
        _selector = selector;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Yeni shipment oluþturuluyor: {OrderId}", request.OrderId);

        var carrier = await _selector.SelectBestCarrierAsync(request.City, request.Weight, request.PreferredDeliveryTimeInHours);

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            City = request.City,
            Weight = request.Weight,
            PreferredDeliveryTimeInHours = request.PreferredDeliveryTimeInHours,
            AssignedCarrierCompanyId = carrier?.Id
        };

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync(cancellationToken);

        if (carrier != null)
        {
            await _publishEndpoint.Publish(new ShipmentAssignedEvent
            {
                ShipmentId = shipment.Id,
                CarrierCompanyName = carrier.Name,
                City = shipment.City
            });

            _logger.LogInformation("Shipment {ShipmentId} icin {Carrier} atandi ve event firlatildi.", shipment.Id, carrier.Name);
        }
        else
        {
            _logger.LogWarning("Shipment {ShipmentId} icin uygun tasiyici bulunamadi", shipment.Id);
           
        }

        return shipment.Id;
    }
}
