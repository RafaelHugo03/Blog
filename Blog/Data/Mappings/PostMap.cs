using System.Collections.Generic;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            //Tabela
           builder.ToTable("Post");
           
           // Primary Key
           builder.HasKey(pk => pk.Id);

           // Identity seed 
           builder.Property(pk => pk.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            //Propriedade LastUpdateDate
           builder.Property(x => x.LastUpdateDate)
           .IsRequired()
           .HasColumnName("LastUpdateDate")
           .HasColumnType("SMALLDATETIME")
           .HasDefaultValueSql("GETDATE()");

           //Index 
           builder.HasIndex(x => x.Slug, "IX_Post_Slug").IsUnique();

           // Relacionamento um para muitos     
           builder.HasOne(x => x.Author)
           .WithMany(x => x.Posts)
           .HasConstraintName("FK_Post_Author")
           .OnDelete(DeleteBehavior.Cascade);
           // Relacionamento um para muitos   
           builder.HasOne(x => x.Category).
           WithMany(x => x.Posts)
           .HasConstraintName("FK_Post_Category")
           .OnDelete(DeleteBehavior.Cascade);
           // relacionamento muitos para muitos 
           builder.HasMany(x => x.Tags)
           .WithMany(x => x.Posts).UsingEntity<Dictionary<string, object>>(
               "PostTag", 
               post => post.HasOne<Tag>()
               .WithMany()
               .HasForeignKey("PostId")
               .HasConstraintName("FK_PostTag_PostId")
               .OnDelete(DeleteBehavior.Cascade),
               tag => tag.HasOne<Post>()
               .WithMany()
               .HasForeignKey("TagId")
               .HasConstraintName("FK_PostTag_TagId")
               .OnDelete(DeleteBehavior.Cascade)
            );
        }
    }
}