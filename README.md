# Smart Shipping System

Akıllı kargo yönlendirme sistemi – taşıyıcı şirketlerin teslimat sürelerine ve kapasite uygunluklarına göre en uygun şirketi seçer. Event-driven mimari kullanılarak, dağıtık sistem yapısına hazır olacak şekilde tasarlanmıştır.

---

## Projeyi Başlatmak

Projeyi Docker ile çalıştırmak için ana dizinde şu komutu çalıştırın:

```bash
docker-compose up --build
```

Bu komut:
- PostgreSQL
- Redis
- RabbitMQ
- SmartShipping.API (ASP.NET Core Web API)

...servislerini birlikte ayağa kaldırır.

---

## Shipment Süreci

### Yeni Shipment Oluşturma
Bir `Shipment` POST isteği atıldığında:

- Uygun kargo şirketi varsa (şehre ve ağırlığa göre):
  - RabbitMQ'ya bir `ShipmentAssignedEvent` fırlatılır
  - Event exchange olarak yayınlanır
- Eğer uygun şirket bulunamazsa:
  - Hangfire ile background job planlanır (5 dakika sonra tekrar denenecek) (Bu kısım henüz tamamlanmadı)

### Shipment Sorgulama
Bir `Shipment` ID ile sorgulanırsa:

- Redis cache'e yazılır
- 5 dakika boyunca (`TimeSpan.FromMinutes(5)`) cache'te tutulur
- Aynı ID ile gelen sonraki isteklerde veri Redis'ten döner

---

## Loglama

Aşağıdaki işlemler `ILogger` kullanılarak loglanır:

- Yeni shipment oluşturma
- Uygun taşıyıcı bulunup bulunamaması
- Event publish edilmesi
- Retry işlemleri

Loglar Docker terminalinde `docker logs <container_id>` ile izlenebilir.

---

## Redis Cache Kontrolü

Redis cache'deki verilere erişmek için şu adımlar izlenebilir:

1. Redis container’ına terminalden bağlan:
```bash
docker exec -it <redis_container_name> redis-cli
```

2. `shipment:*` anahtarlarını listele:
```redis
keys shipment:*
```

3. İlgili shipment'ı görüntülemek için:
```redis
get shipment:<shipment-id>
```

---

## RabbitMQ Event Takibi

RabbitMQ UI arayüzüne erişmek için:

`http://localhost:15672`  
**Kullanıcı adı:** `guest`  
**Şifre:** `guest`

`SmartShipping.Shared.Events:ShipmentAssignedEvent` adlı exchange altında event'ler izlenebilir.

---

## Teknolojiler

- ASP.NET Core 8 + CQRS + MediatR
- PostgreSQL (EF Core)
- Redis (StackExchange.Redis)
- RabbitMQ + MassTransit
- Hangfire (Retry işlemleri için) (Henüz eklenmedi)
- Docker Compose
- ILogger + Middleware (hata/log yönetimi)

---

## Test

Swagger üzerinden API endpoint'lerini test etmek için:

👉 `http://localhost:5000/swagger`

---
