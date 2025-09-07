// NatureAPI/Data/NatureApiContext.cs

using Microsoft.EntityFrameworkCore;
using NatureAPI.Models.Entities;

namespace NatureAPI.Data;

public class NatureApiContext : DbContext
{
    public NatureApiContext(DbContextOptions<NatureApiContext> options) : base(options)
    {
    }

    // DbSet para cada entidad -> tablas en la BD.
    public DbSet<Place> Places { get; set; }
    public DbSet<Trail> Trails { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    
    //  DbSet para tabla puente
    public DbSet<PlaceAmenity> PlaceAmenities { get; set; }

    // onfiguramos las relaciones complejas
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuracion de la relacion

        //clave primaria compuesta para la tabla puente PlaceAmenity
        modelBuilder.Entity<PlaceAmenity>()
            .HasKey(pa => new { pa.PlaceId, pa.AmenityId });

        // relacion de place a PlaceAmenity
        modelBuilder.Entity<PlaceAmenity>()
            // PlaceAmenity tiene un Place
            .HasOne(pa => pa.Place) 
            // Place tiene muchas PlaceAmenities
            .WithMany(p => p.PlaceAmenities) 
            // clave foronea es PlaceId
            .HasForeignKey(pa => pa.PlaceId);

        // relacion de Amenity a PlaceAmenity
        modelBuilder.Entity<PlaceAmenity>()
            // Una PlaceAmenity tiene una Amenity
            .HasOne(pa => pa.Amenity)
            //Amenity tiene muchas PlaceAmenities
            .WithMany(a => a.PlaceAmenities) 
            // La clave forea es AmenityId
            .HasForeignKey(pa => pa.AmenityId);
    }
}