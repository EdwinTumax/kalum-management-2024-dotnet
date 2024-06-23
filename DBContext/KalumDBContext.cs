using System.Numerics;
using KalumManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace KalumManagement.DBContext
{
    public class KalumDBContext : DbContext
    {
        public DbSet<CarreraTecnica> CarrerasTecnicas {get;set;}
        public DbSet<Jornada> Jornadas {get;set;}
        public DbSet<ExamenAdmision> ExamenesAdmision {get;set;}
        public DbSet<ResultadoExamenAdmision> ResultadoExamenAdmisions {get;set;}
        public DbSet<Aspirante> Aspirantes {get;set;}
        public DbSet<Alumno> Alumnos {get;set;}
        public DbSet<CuentasXCobrar> CuentasXCobrar;
        public DbSet<Cargo> Cargos {get;set;}
        public DbSet<InscripcionPago> InscripcionesPago {get;set;}
        public DbSet<Inscripcion> Inscripciones {get;set;}
        public DbSet<InversionCarreraTecnica> InversionCarrerasTecnicas {get;set;}
        public KalumDBContext(DbContextOptions options) : base(options)        
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new {ct.CarreraId});
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new {j.JornadaId});
            modelBuilder.Entity<ExamenAdmision>().ToTable("ExamenAdmision").HasKey(ea => new {ea.ExamenId});
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new {a.NoExpediente});
            modelBuilder.Entity<ResultadoExamenAdmision>().ToTable("ResultadoExamenAdmision").HasKey(re => new {re.NoExpediente, re.Anio});
            modelBuilder.Entity<Alumno>().ToTable("Alumno").HasKey(a => new {a.Carne});
            modelBuilder.Entity<CuentasXCobrar>().ToTable("CuentasXCobrar").HasKey(cxp => new {cxp.Cargo, cxp.Anio, cxp.Carne});
            modelBuilder.Entity<Cargo>().ToTable("Cargo").HasKey(c => new {c.CargoId});
            modelBuilder.Entity<InscripcionPago>().ToTable("InscripcioPago").HasKey(ip => new {ip.BoletaPago, ip.NoExpediente, ip.Anio});
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion").HasKey(i => new {i.InscripcionId});
            modelBuilder.Entity<InversionCarreraTecnica>().ToTable("InversionCarreraTecnica").HasKey(ict => new {ict.InversionId});

            modelBuilder.Entity<Aspirante>()
                .HasOne<CarreraTecnica>(a => a.CarreraTecnica)
                .WithMany(ct => ct.Aspirantes)
                .HasForeignKey(a => a.CarreraId);
            modelBuilder.Entity<Aspirante>()
                .HasOne<ExamenAdmision>(a => a.ExamenAdmision)
                .WithMany(ea => ea.Aspirantes)
                .HasForeignKey(a => a.ExamenId);
            modelBuilder.Entity<Aspirante>()
                .HasOne<Jornada>(a => a.Jornada)
                .WithMany(j => j.Aspirantes)
                .HasForeignKey(a => a.JornadaId);
            modelBuilder.Entity<ResultadoExamenAdmision>()
                .HasOne<Aspirante>(re => re.Aspirante)
                .WithMany(a => a.ResultadoExamenAdmision)
                .HasForeignKey(re => re.NoExpediente);
            modelBuilder.Entity<CuentasXCobrar>()
                .HasOne<Alumno>(cxp => cxp.Alumno)
                .WithMany(a => a.CuentasXCobrar)
                .HasForeignKey(cxp => cxp.Carne);
            modelBuilder.Entity<CuentasXCobrar>()
                .HasOne<Cargo>(cxp => cxp.CargoAplicado)
                .WithMany(c => c.CuentasXCobrar)
                .HasForeignKey(cxp => cxp.CargoId);
            modelBuilder.Entity<InscripcionPago>()
                .HasOne<Aspirante>(ip => ip.Aspirante)
                .WithMany(a => a.InscripcionesPago)
                .HasForeignKey(ip => ip.NoExpediente);
            modelBuilder.Entity<Inscripcion>()
                .HasOne<Alumno>(i => i.Alumno)
                .WithMany(a => a.Inscripciones)
                .HasForeignKey(i => i.Carne);
            modelBuilder.Entity<Inscripcion>()
                .HasOne<CarreraTecnica>(i => i.CarreraTecnica)
                .WithMany(ct => ct.Inscripciones)
                .HasForeignKey(i => i.CarreraId);
            modelBuilder.Entity<Inscripcion>()
                .HasOne<Jornada>(i => i.Jornada)
                .WithMany(a => a.Inscripciones)
                .HasForeignKey(i => i.JornadaId);
            modelBuilder.Entity<InversionCarreraTecnica>()
                .HasOne<CarreraTecnica>(ict => ict.CarreraTecnica)
                .WithMany(ct => ct.InversionCarrerasTecnicas)
                .HasForeignKey(ict => ict.CarreraId);
        }

    }
}