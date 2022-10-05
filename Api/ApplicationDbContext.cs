using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().ToTable("Products");
        builder.Entity<Category>().ToTable("Categories");
        builder.Entity<Tag>().ToTable("Tags");

        builder.Entity<Product>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Entity<Product>().Property(p => p.Code).HasMaxLength(20).IsRequired();
        builder.Entity<Product>().Property(p => p.Description).HasMaxLength(500).IsRequired(false);
    }
}
