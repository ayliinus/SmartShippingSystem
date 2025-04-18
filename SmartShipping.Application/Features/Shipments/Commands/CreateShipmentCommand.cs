using MediatR;

namespace SmartShipping.Application.Features.Shipments.Commands;

public class CreateShipmentCommand : IRequest<Guid>
{
    public string OrderId { get; set; } = default!;
    public string City { get; set; } = default!;
    public double Weight { get; set; }
    public int PreferredDeliveryTimeInHours { get; set; }
}
