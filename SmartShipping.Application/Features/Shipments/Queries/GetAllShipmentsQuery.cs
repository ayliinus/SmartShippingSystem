using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SmartShipping.Application.Features.Shipments.DTOs;

namespace SmartShipping.Application.Features.Shipments.Queries
{
    public class GetAllShipmentsQuery : IRequest<List<ShipmentDto>>
    {
    }
}
