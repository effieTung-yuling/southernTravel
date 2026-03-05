namespace southernTravel.Data
{
    using southernTravel.Model;
    using Microsoft.EntityFrameworkCore;
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Products> Products { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}
