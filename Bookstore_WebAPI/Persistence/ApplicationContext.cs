using Bookstore_WebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookstore_WebAPI.Persistence
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> optins) : base(optins) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<PublishingHouse> PublishingHouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new PublishingHouseConfiguration());
        }

        public class AuthorConfiguration : IEntityTypeConfiguration<Author>
        {
            public void Configure(EntityTypeBuilder<Author> builder)
            {
                builder.Property(e => e.FirstName).HasMaxLength(30).IsRequired();
                builder.Property(e => e.LastName).HasMaxLength(30).IsRequired();
                builder.Property(e => e.Id).HasColumnName("author_Id");
            }
        }

        public class BookConfiguration : IEntityTypeConfiguration<Book>
        {
            public void Configure(EntityTypeBuilder<Book> builder)
            {
                builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
                builder.HasOne(e => e.PublishingHouse).WithOne(e => e.Book).HasForeignKey<PublishingHouse>(e => e.BookId).OnDelete(DeleteBehavior.Cascade);
                builder.Property(e => e.Id).HasColumnName("book_Id");
            }
        }

        public class PublishingHouseConfiguration : IEntityTypeConfiguration<PublishingHouse>
        {
            public void Configure(EntityTypeBuilder<PublishingHouse> builder)
            {
                builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
                builder.Property(e => e.Id).HasColumnName("publishinghouse_Id");
            }
        }
    }
}