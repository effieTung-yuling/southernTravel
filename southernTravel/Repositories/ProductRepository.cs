using southernTravel.Data;
using southernTravel.Model;
using Microsoft.EntityFrameworkCore;

namespace southernTravel.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

    }
}
