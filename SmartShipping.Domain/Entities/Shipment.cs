namespace SmartShipping.Domain.Entities;

public class Shipment
{
    public Guid Id { get; set; }
    public string OrderId { get; set; } = default!;
    public string City { get; set; } = default!;
    public double Weight { get; set; }
    public int PreferredDeliveryTimeInHours { get; set; }

    public Guid? AssignedCarrierCompanyId { get; set; }
    public CarrierCompany? AssignedCarrierCompany { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}