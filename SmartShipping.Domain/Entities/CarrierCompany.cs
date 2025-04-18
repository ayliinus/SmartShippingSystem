namespace SmartShipping.Domain.Entities;

public class CarrierCompany
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int DeliveryTimeInHours { get; set; }
    public int Capacity { get; set; }
    public string? SupportedCities { get; set; }

    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}