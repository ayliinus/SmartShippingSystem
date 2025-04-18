using MediatR;
using SmartShipping.Application.Features.Shipments.DTOs;

namespace SmartShipping.Application.Features.Shipments.Queries;

public class GetShipmentByIdQuery : IRequest<ShipmentDto>
{
    public Guid Id { get; set; }

    public GetShipmentByIdQuery(Guid id)
    {
        Id = id;
    }
}
