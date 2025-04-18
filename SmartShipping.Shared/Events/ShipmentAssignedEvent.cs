namespace SmartShipping.Shared.Events;

public class ShipmentAssignedEvent
{
    public Guid ShipmentId { get; set; }
    public string CarrierCompanyName { get; set; } = default!;
    public string City { get; set; } = default!;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}