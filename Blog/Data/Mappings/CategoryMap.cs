using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Tabela
            builder.ToTable("Category");

            //Primary Key
            builder.HasKey(pk => pk.Id);

            // identity
            builder.Property(pk => pk.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(); // Identity (1,1)

            //Propriedades

            builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

            builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

            // Indices
            builder.HasIndex(x => x.Slug, "IX_Category_Slug").IsUnique();
        }
    }
}