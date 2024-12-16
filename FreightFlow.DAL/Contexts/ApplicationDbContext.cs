using System;
using System.Collections.Generic;
using FreightFlow.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FreightFlow.DAL.Contexts;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.Cargoid).HasName("cargo_pkey");

            entity.ToTable("cargo");

            entity.Property(e => e.Cargoid).HasColumnName("cargoid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Specialrequirements).HasColumnName("specialrequirements");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.Volume)
                .HasPrecision(10, 2)
                .HasColumnName("volume");
            entity.Property(e => e.Weight)
                .HasPrecision(10, 2)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Clientid).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Companyname)
                .HasMaxLength(100)
                .HasColumnName("companyname");
            entity.Property(e => e.Contactperson)
                .HasMaxLength(100)
                .HasColumnName("contactperson");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Inn)
                .HasMaxLength(12)
                .HasColumnName("inn");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Driverid).HasName("drivers_pkey");

            entity.ToTable("drivers");

            entity.Property(e => e.Driverid).HasColumnName("driverid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Licenseexpiry).HasColumnName("licenseexpiry");
            entity.Property(e => e.Licensenumber)
                .HasMaxLength(50)
                .HasColumnName("licensenumber");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Available'::character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Cargoid).HasColumnName("cargoid");
            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Deliverydate).HasColumnName("deliverydate");
            entity.Property(e => e.Driverid).HasColumnName("driverid");
            entity.Property(e => e.Orderdate).HasColumnName("orderdate");
            entity.Property(e => e.Routeid).HasColumnName("routeid");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'New'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Vehicleid).HasColumnName("vehicleid");

            entity.HasOne(d => d.Cargo).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Cargoid)
                .HasConstraintName("orders_cargoid_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Clientid)
                .HasConstraintName("orders_clientid_fkey");

            entity.HasOne(d => d.Driver).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Driverid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("orders_driverid_fkey");

            entity.HasOne(d => d.Route).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Routeid)
                .HasConstraintName("orders_routeid_fkey");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Vehicleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("orders_vehicleid_fkey");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Routeid).HasName("routes_pkey");

            entity.ToTable("routes");

            entity.Property(e => e.Routeid).HasColumnName("routeid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Distance)
                .HasPrecision(10, 2)
                .HasColumnName("distance");
            entity.Property(e => e.Endpoint)
                .HasMaxLength(100)
                .HasColumnName("endpoint");
            entity.Property(e => e.Intermediatepoints).HasColumnName("intermediatepoints");
            entity.Property(e => e.Startpoint)
                .HasMaxLength(100)
                .HasColumnName("startpoint");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Tripid).HasName("trips_pkey");

            entity.ToTable("trips");

            entity.Property(e => e.Tripid).HasColumnName("tripid");
            entity.Property(e => e.Driverid).HasColumnName("driverid");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'In Progress'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Vehicleid).HasColumnName("vehicleid");

            entity.HasOne(d => d.Driver).WithMany(p => p.Trips)
                .HasForeignKey(d => d.Driverid)
                .HasConstraintName("trips_driverid_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Trips)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("trips_orderid_fkey");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Trips)
                .HasForeignKey(d => d.Vehicleid)
                .HasConstraintName("trips_vehicleid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Vehicleid).HasName("vehicles_pkey");

            entity.ToTable("vehicles");

            entity.Property(e => e.Vehicleid).HasColumnName("vehicleid");
            entity.Property(e => e.Loadcapacity)
                .HasPrecision(10, 2)
                .HasColumnName("loadcapacity");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Available'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Vehiclenumber)
                .HasMaxLength(20)
                .HasColumnName("vehiclenumber");
            entity.Property(e => e.Vehicletype)
                .HasMaxLength(20)
                .HasColumnName("vehicletype");
            entity.Property(e => e.Volume)
                .HasPrecision(10, 2)
                .HasColumnName("volume");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
