using Microsoft.EntityFrameworkCore;
using SmartShipping.Application.Interfaces;
using SmartShipping.Domain.Entities;

namespace SmartShipping.Infrastructure.Services;

public class CarrierSelectorService : ICarrierSelectorService
{
    private readonly IApplicationDbContext _context;

    public CarrierSelectorService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CarrierCompany?> SelectBestCarrierAsync(string city, double weight, int preferredTime)
    {
        var candidates = await _context.CarrierCompanies
            .Where(c =>
                c.SupportedCities!.Contains(city) &&
                c.DeliveryTimeInHours <= preferredTime &&
                c.Capacity > 0
            ).ToListAsync();

        return candidates
            .OrderBy(c => c.DeliveryTimeInHours)
            .FirstOrDefault();
    }
}
