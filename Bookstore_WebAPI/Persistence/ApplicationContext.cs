using Bookstore_WebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Bookstore_WebAPI.Persistence
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> optins) : base(optins) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<PublishingHouse> PublishingHouses { get; set; }
        public DbSet<AuthorBooks> AuthorBooks { get; set; }
        public DbSet<AuthorPublishingHouses> AuthorPublishingHouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new PublishingHouseConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorBooksConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorPublishingHousesConfiguration());
        }

        public class AuthorPublishingHousesConfiguration : IEntityTypeConfiguration<AuthorPublishingHouses>
        {
            public void Configure(EntityTypeBuilder<AuthorPublishingHouses> builder)
            {
                builder
                    .HasOne(aph => aph.Author)
                    .WithMany(a => a.AuthorPublishingHouses)
                    .HasForeignKey(aph => aph.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
                builder
                    .HasOne(p => p.PublishingHouse)
                    .WithMany(pc => pc.AuthorsPublishingHouses)
                    .HasForeignKey(c => c.PublishingHouseId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }

        public class AuthorBooksConfiguration : IEntityTypeConfiguration<AuthorBooks>
        {
            public void Configure(EntityTypeBuilder<AuthorBooks> builder)
            {
                builder
                    .HasOne(ab => ab.Author)
                    .WithMany(a => a.AuthorBooks)
                    .HasForeignKey(ab => ab.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
                builder
                    .HasOne(ab => ab.Book)
                    .WithMany(b => b.AuthorBooks)
                    .HasForeignKey(ab => ab.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }

        public class AuthorConfiguration : IEntityTypeConfiguration<Author>
        {
            public void Configure(EntityTypeBuilder<Author> builder)
            {
                builder
                    .Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .IsRequired();
                builder
                    .Property(e => e.LastName)
                    .HasMaxLength(30)
                    .IsRequired();
            }
        }

        public class BookConfiguration : IEntityTypeConfiguration<Book>
        {
            public void Configure(EntityTypeBuilder<Book> builder)
            {
                builder
                    .Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsRequired();
                builder
                    .HasOne(b => b.PublishingHouse)
                    .WithMany(ph => ph.Books)
                    .HasForeignKey(b => b.PublishingHouseId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }

        public class PublishingHouseConfiguration : IEntityTypeConfiguration<PublishingHouse>
        {
            public void Configure(EntityTypeBuilder<PublishingHouse> builder)
            {
                builder
                    .Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsRequired();
            }
        }
    }
}
