using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Configuration;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.Items)
               .WithOne(i => i.Cart)
               .HasForeignKey(i => i.CartId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}