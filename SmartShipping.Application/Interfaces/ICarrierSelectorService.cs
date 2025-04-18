using SmartShipping.Domain.Entities;

namespace SmartShipping.Application.Interfaces;

public interface ICarrierSelectorService
{
    Task<CarrierCompany?> SelectBestCarrierAsync(string city, double weight, int preferredTime);
}
