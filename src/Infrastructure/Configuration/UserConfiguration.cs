using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasOne(c => c.Cart)
               .WithOne(c => c.User)
               .HasForeignKey<Cart>(c => c.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}