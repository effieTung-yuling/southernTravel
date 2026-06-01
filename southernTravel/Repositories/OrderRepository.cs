using Microsoft.EntityFrameworkCore;
using southernTravel.Data;
using southernTravel.Model;
using southernTravel.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Orders>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }
}

