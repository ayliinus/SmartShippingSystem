using Microsoft.EntityFrameworkCore;
using SmartShipping.Domain.Entities;

namespace SmartShipping.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Shipment> Shipments { get; }
    DbSet<CarrierCompany> CarrierCompanies { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
