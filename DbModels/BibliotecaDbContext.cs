using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BibliotecaApi.DbModels;
public partial class BibliotecaDbContext : IdentityDbContext
{
    public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Editorial> Editoriales { get; set; }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Prestamo> Prestamos { get; set; }
    public DbSet<AppLog> AppLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
       
        base.OnModelCreating(builder);

        builder.Entity<Autor>(entity =>
        {
            entity.ToTable("Autores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.Estado)
            .IsRequired()
            .HasDefaultValue(true);

         
        });

        builder.Entity<Editorial>(entity =>
        {
            entity.ToTable("Editoriales");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.Estado)
            .IsRequired()
            .HasDefaultValue(true);
        });
       
        builder.Entity<Libro>(entity =>
        {
            entity.ToTable("Libros");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.Descripcion)
                .IsRequired()
                .HasColumnType("varchar(max)");
            entity.Property(e => e.Copias)
                .IsRequired()
                .HasDefaultValue(1);
            entity.Property(e => e.Fecha_Publicacion)
                .HasColumnType("date");
            entity.Property(e => e.Estado)
            .IsRequired()
            .HasDefaultValue(true);
            entity.HasOne(l => l.Editorial)
            .WithMany()
            .HasForeignKey(l => l.Id_Editorial);
            entity.HasOne(l => l.Autor)
            .WithMany()
            .HasForeignKey(l => l.Id_Autor);
        });

        builder.Entity<Prestamo>(entity =>
        {
            entity.ToTable("Prestamos");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Fecha_Prestamo)
            .HasColumnType("datetime");

            entity.Property(e => e.Fecha_Devolucion_Esperada)
            .HasColumnType("datetime");

            entity.Property(e => e.Fecha_Devolucion_Real)
            .HasColumnType("datetime");

            entity.HasOne(l => l.Libro)
            .WithMany()
            .HasForeignKey(l => l.Id_Libro)
            .IsRequired();

             entity.HasOne(l => l.Usuario)
            .WithMany()
            .HasForeignKey(l => l.Id_Usuario)
            .IsRequired();

            entity.Property(e => e.Estado)
            .IsRequired()
            .HasDefaultValue(true);
        });

        builder.Entity<AppLog>(entity =>
        {
            entity.ToTable("AppLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CallSite)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Controller)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Exceptcion).IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Level)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Logger)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.MachineName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Message).IsUnicode(false);
            entity.Property(e => e.RequestIp)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.RequestUrl)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TraceId)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
       
    }


}
