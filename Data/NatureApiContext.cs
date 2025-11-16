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
          new Photo { Id = 1, PlaceId = 1, Url = "https://www.mexicodesconocido.com.mx/wp-content/uploads/2016/12/nevado-de-toluca-parque-1600.jpg" },
          new Photo { Id = 2, PlaceId = 1, Url = "https://cdn.visitmexico.com/sites/default/files/styles/explore_hero_desktop/public/2020-03/Nevado-Toluca.jpg" },
          new Photo { Id = 3, PlaceId = 1, Url = "https://escapadas.mexicodesconocido.com.mx/wp-content/uploads/2020/10/nevado-de-toluca-foto-fernando-acosta.jpg" },
          new Photo { Id = 4, PlaceId = 1, Url = "https://volcanesdemexico.org/wp-content/uploads/2018/12/clima-nevado-de-toluca.jpg" },

          // Lugar 2
          new Photo { Id = 5, PlaceId = 2, Url = "https://cloudfront-us-east-1.images.arcpublishing.com/infobae/6VJHMRXIY5HKDGJ775PGPVH65Q.jpg" },
          new Photo { Id = 6, PlaceId = 2, Url = "https://www.mexicodesconocido.com.mx/wp-content/uploads/2017/09/Cascadas-de-Tamul-San-Luis-Potosi_1200.jpg" },
          new Photo { Id = 7, PlaceId = 2, Url = "https://www.mexicodesconocido.com.mx/wp-content/uploads/2017/01/Cascada-de-Tamasopo-San-Luis-Potosi_1920p.jpg" },
          new Photo { Id = 8, PlaceId = 2, Url = "https://www.civitatis.com/f/mexico/san-luis-potosi/excursion-cascada-tamul-cueva-agua-589x392.jpg" },

          // Lugar 3
          new Photo { Id = 9, PlaceId = 3, Url = "https://www.caminoreal.com/storage/app/media/Blog/la-bufadora-baja-california.jpg" },
          new Photo { Id = 10, PlaceId = 3, Url = "https://pacifica-ensenada.com/wp-content/uploads/2024/09/1000_F_347652456_i1yUhivVTHJowe9ItdXRBQntdPbc6jNe.jpg" },
          new Photo { Id = 11, PlaceId = 3, Url = "https://www.debate.com.mx/__export/1727036211378/sites/debate/img/2024/09/22/la_bufadora.jpg_466078407.jpg" },
          new Photo { Id = 12, PlaceId = 3, Url = "https://mediaim.expedia.com/destination/2/9a586ef50a9488138b6a62269172663b.jpg" }
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
