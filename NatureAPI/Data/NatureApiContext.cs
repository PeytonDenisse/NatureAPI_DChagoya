using Microsoft.EntityFrameworkCore;
using NatureAPI.Models.Entities;

namespace NatureAPI.Data;

public class NatureDbContext : DbContext
{
    public NatureDbContext(DbContextOptions<NatureDbContext> options) : base(options) { }

    public DbSet<Place> Places => Set<Place>();
    public DbSet<Trail> Trails => Set<Trail>();
    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<PlaceAmenity> PlaceAmenities => Set<PlaceAmenity>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // PlaceAmenity PK compuesta
        mb.Entity<PlaceAmenity>()
          .HasKey(pa => new { pa.PlaceId, pa.AmenityId });

        // Relaciones 1–N
        mb.Entity<Trail>()
          .HasOne(t => t.Place)
          .WithMany(p => p.Trails)
          .HasForeignKey(t => t.PlaceId)
          .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Photo>()
          .HasOne(ph => ph.Place)
          .WithMany(p => p.Photos)
          .HasForeignKey(ph => ph.PlaceId)
          .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Review>()
          .HasOne(r => r.Place)
          .WithMany(p => p.Reviews)
          .HasForeignKey(r => r.PlaceId)
          .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<PlaceAmenity>()
          .HasOne(pa => pa.Place)
          .WithMany(p => p.PlaceAmenities)
          .HasForeignKey(pa => pa.PlaceId);

        mb.Entity<PlaceAmenity>()
          .HasOne(pa => pa.Amenity)
          .WithMany(a => a.PlaceAmenities)
          .HasForeignKey(pa => pa.AmenityId);

        // === SEED ===  (todo excepto Review)
        mb.Entity<Place>().HasData(
          new Place {
            Id=1, Name="Parque Nacional Nevado de Toluca", Category="Parque",
            Description="Zona volcánica con lagunas en el cráter.",
            Latitude=19.1083, Longitude=-99.7589, ElevationMeters=4000,
            Accessible=true, EntryFee=54, OpeningHours="08:00-17:00",
            CreatedAt=new DateTime(2024,1,10)
          },
          new Place {
            Id=2, Name="Cascada de Tamul", Category="Cascada",
            Description="Imponente caída de agua en la Huasteca Potosina.",
            Latitude=21.6937, Longitude=-99.1827, ElevationMeters=300,
            Accessible=true, EntryFee=40, OpeningHours="08:00-18:00",
            CreatedAt=new DateTime(2024,2,15)
          },
          new Place {
            Id=3, Name="La Bufadora", Category="Mirador",
            Description="Geyser marino/rezonador en la costa de Ensenada.",
            Latitude=31.7457, Longitude=-116.7147, ElevationMeters=20,
            Accessible=true, EntryFee=0, OpeningHours="Abierto",
            CreatedAt=new DateTime(2024,3,5)
          }
        );

        mb.Entity<Trail>().HasData(
          new Trail { Id=1, PlaceId=1, Name="Sendero al Cráter", DistanceKm=12, EstimatedTimeMinutes=300, Difficulty="Alta", Path="[ [19.12,-99.76],[19.11,-99.75] ]", IsLoop=false },
          new Trail { Id=2, PlaceId=2, Name="Mirador de Tamul",  DistanceKm=4.5, EstimatedTimeMinutes=120, Difficulty="Media", Path="[ [21.69,-99.19],[21.69,-99.18] ]", IsLoop=false },
          new Trail { Id=3, PlaceId=3, Name="Costero La Bufadora", DistanceKm=1.2, EstimatedTimeMinutes=45, Difficulty="Baja", Path="[ [31.74,-116.71],[31.75,-116.71] ]", IsLoop=true }
        );

        mb.Entity<Photo>().HasData(
          new Photo { Id=1, PlaceId=1, Url="https://upload.wikimedia.org/wikipedia/commons/6/69/Nevado_de_Toluca_crater_lakes.jpg" },
          new Photo { Id=2, PlaceId=2, Url="https://upload.wikimedia.org/wikipedia/commons/8/85/Cascada_de_Tamul_SLP.jpg"},
          new Photo { Id=3, PlaceId=3, Url="https://upload.wikimedia.org/wikipedia/commons/7/70/La_Bufadora_Blowhole.jpg"}
        );

        mb.Entity<Amenity>().HasData(
          new Amenity { Id=1, Name="Baños" },
          new Amenity { Id=2, Name="Estacionamiento" },
          new Amenity { Id=3, Name="Mirador" },
          new Amenity { Id=4, Name="Área de picnic" }
        );

        mb.Entity<PlaceAmenity>().HasData(
          new PlaceAmenity { PlaceId=1, AmenityId=1 },
          new PlaceAmenity { PlaceId=1, AmenityId=2 },
          new PlaceAmenity { PlaceId=1, AmenityId=3 },
          new PlaceAmenity { PlaceId=2, AmenityId=1 },
          new PlaceAmenity { PlaceId=2, AmenityId=3 },
          new PlaceAmenity { PlaceId=3, AmenityId=2 },
          new PlaceAmenity { PlaceId=3, AmenityId=3 }
        );
    }
}
