using southernTravel.Model;

namespace southernTravel.Repositories
{
    public interface IAttractionRepository
    {
        Task<List<Attraction>> GetAllAsync();

        Task<Attraction?> GetByIdAsync(int id);

        Task<Attraction> CreateAsync(Attraction attraction);

        Task UpdateAsync(Attraction attraction);

        Task DeleteAsync(Attraction attraction);
    }
}
