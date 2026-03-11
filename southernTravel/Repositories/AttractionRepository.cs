using Microsoft.EntityFrameworkCore;
using southernTravel.Data;
using southernTravel.Model;
using southernTravel.Repositories;

public class AttractionRepository : IAttractionRepository
{
    private readonly AppDbContext _context;

    public AttractionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Attraction>> GetAllAsync()
    {
        return await _context.Attractions.ToListAsync();
    }

    public async Task<Attraction?> GetByIdAsync(int id)
    {
        return await _context.Attractions.FindAsync(id);
    }

    public async Task<Attraction> CreateAsync(Attraction attraction)
    {
        _context.Attractions.Add(attraction);
        await _context.SaveChangesAsync();
        return attraction;
    }

    public async Task UpdateAsync(Attraction attraction)
    {
        _context.Attractions.Update(attraction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Attraction attraction)
    {
        _context.Attractions.Remove(attraction);
        await _context.SaveChangesAsync();
    }
}