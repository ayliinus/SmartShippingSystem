namespace SmartShipping.Application.Features.Shipments.DTOs;

public class ShipmentDto
{
    public Guid Id { get; set; }
    public string OrderId { get; set; } = default!;
    public string City { get; set; } = default!;
    public double Weight { get; set; }
    public int PreferredDeliveryTimeInHours { get; set; }
    public string? CarrierCompanyName { get; set; }
}
