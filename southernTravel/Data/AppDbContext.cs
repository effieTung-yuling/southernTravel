namespace southernTravel.Data
{
    using southernTravel.Model;
    using Microsoft.EntityFrameworkCore;
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Itinerary> Itineraries { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
