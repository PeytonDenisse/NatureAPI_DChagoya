# NatureAPI_DChagoya

# NatureAPI

API REST en **.NET 8** para administrar **lugares naturales de México** (parques, cascadas, miradores y senderos).  
Usa **SQL Server (Docker)**, **Entity Framework Core** (migraciones + seed) y **Swagger**.

---

## ¿Qué hace?
- Guarda **lugares** con coordenadas y metadatos.
- Relaciones:
  - Place **1–N** Trail, Photo, Review
  - Place **N–N** Amenity mediante la tabla puente **PlaceAmenity**
- Incluye **datos de ejemplo (seed)** para Place, Trail, Photo, Amenity y PlaceAmenity.

---

## Requisitos
- **.NET 8 SDK**
- **Docker / Docker Compose**
- (Opcional) **Azure Data Studio** para ver la BD
- 
---

## NatureAPI/ estructura 

NatureAPI/
  Controllers/PlacesController.cs
  Data/NatureDbContext.cs
  Models/PlaceCreateDto.cs
  Program.cs
NatureAPI.Models/
  Entities/(Place, Trail, Photo, Review, Amenity, PlaceAmenity)
compose.yaml


---

## Endpoints principales

GET /api/places
Filtros: ?category=Parque&difficulty=Baja

GET /api/places/{id}

POST /api/places

---
## Migraciones y seed

Crea la BD y carga los datos iniciales.

dotnet tool install --global dotnet-ef   # si no la tienes
dotnet ef migrations add InitialCreate --project NatureAPI --startup-project NatureAPI
dotnet ef database update --project NatureAPI --startup-project NatureAPI
---

## 1) Base de datos (Docker)
El repo incluye `compose.yaml`. :

```bash
docker compose up -d



