using CleanArchitecture.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.EntityConfigurations.Identities;

public static class IdentityConfigureEntities
{
    public static ModelBuilder ConfigureCoreIdentityEntities(this ModelBuilder modelBuilder, bool migration = true)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(nameof(User), SchemaDbConstants.Auth);
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.UserName).IsUnique();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.DateOfBirth).HasColumnType("date");

        });

        modelBuilder.Entity<RoleClaim>(entity =>
        {
            entity.ToTable(nameof(RoleClaim), SchemaDbConstants.Auth);

            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable(nameof(UserToken), SchemaDbConstants.Auth);
            entity.HasKey(x => new
            {
                x.LoginProvider,
                x.UserId,
                x.Name
            });
        });

        modelBuilder.Entity<UserClaim>(entity =>
        {
            entity.ToTable(nameof(UserClaim), SchemaDbConstants.Auth);
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.ToTable(nameof(UserLogin), SchemaDbConstants.Auth);
            entity.HasKey(x => new
            {
                x.LoginProvider,
                x.UserId
            });
        });
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable(nameof(Role),
                SchemaDbConstants.Auth);
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable(nameof(UserRole), SchemaDbConstants.Auth);
            entity.HasKey(x => new
            {
                x.UserId,
                x.RoleId
            });
            entity
                .HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId)
                ;
            entity
                .HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable(nameof(RefreshToken), SchemaDbConstants.Auth);
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.HasIndex(x => new { x.TokenHash, x.UserId }).IsUnique();

            
            entity.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId);

            entity.Property(x => x.TokenHash).HasMaxLength(255);
        });
        return modelBuilder;
    }
}