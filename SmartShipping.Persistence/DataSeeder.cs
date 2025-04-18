using SmartShipping.Domain.Entities;
using SmartShipping.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        Console.WriteLine("[DataSeeder] Seed işlemi başlatıldı...");
        if (!context.CarrierCompanies.Any())
        {
            var carriers = new List<CarrierCompany>
            {
                new CarrierCompany
                {
                    Id = Guid.NewGuid(),
                    Name = "HızlıKargo",
                    Capacity = 10,
                    DeliveryTimeInHours = 24,
                    SupportedCities =  "Istanbul, Ankara, Izmir" 
                },
                new CarrierCompany
                {
                    Id = Guid.NewGuid(),
                    Name = "YavaşAmaUcuz",
                    Capacity = 20,
                    DeliveryTimeInHours = 48,
                    SupportedCities = "Istanbul, Bursa" 
                }
            };

            context.CarrierCompanies.AddRange(carriers);
            await context.SaveChangesAsync(default);
            Console.WriteLine("[DataSeeder] Veri başarıyla eklendi.");
        }
        else
        {
            Console.WriteLine("[DataSeeder] Zaten veri var, seed işlemi atlandı.");
        }
    }
}
