using Gateway.Helpers;
using Gateway.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Models
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext()
        {
        }

        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Environment.GetEnvironmentVariable(Consts.ConnectionStringEnvVarName) ?? throw new ArgumentException();
                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.24-mariadb"));
            }
        }
        //"server=localhost;port=3306;user=root;database=zti_wyklad"
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_polish_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_token");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ExpirationTime)
                    .HasColumnType("datetime")
                    .HasColumnName("expiration_time");

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .HasColumnName("token");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("refresh_token_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
