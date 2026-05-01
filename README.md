# Akilli Bina Sistemi (ADLE)

C# .NET Framework 4.8 WPF tabanlı Akıllı Bina / ADLE (Activity Driven Life Environments) simülasyon uygulaması.

## Gereksinimler

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Visual Studio 2019+
- .NET Framework 4.8

## Veritabanı Kurulumu (PostgreSQL)

### 1. PostgreSQL'i Docker ile başlat

```bash
docker-compose up -d
```

Container çalışıyor mu kontrol et:

```bash
docker ps
```

Bağlantı bilgileri:
- Host: `localhost`
- Port: `5432`
- Database: `adle_sim`
- Username: `adle_user`
- Password: `Password1`

### 2. Migration çalıştır

Visual Studio'da **Package Manager Console**'u aç:

**ADLE veritabanı:**
```powershell
# Default Project: DatabaseMigration
Update-Database
```

**Simülasyon veritabanı:**
```powershell
# Default Project: SimulationDB_Migrations
Update-Database
```

## Proje Yapısı

| Proje | Açıklama |
|-------|---------|
| `DomainObjects` | Domain entity modelleri |
| `DataAccess` | EF6 generic repository |
| `DatabaseMigration` | ADLE DB migration (Areas, Items, Memories) |
| `SimulationDB_Migrations` | Simülasyon DB migration |
| `GUI` | Ana WPF arayüzü |
| `GUI_Simulation` | Simülasyon WPF arayüzü |
| `FakeDevices` | Sahte cihaz simülasyonu |
| `FakeDataProvider` | Test verisi sağlayıcı |

## Veritabanını Durdur

```bash
docker-compose down
```

Verileri de sil:

```bash
docker-compose down -v
```
