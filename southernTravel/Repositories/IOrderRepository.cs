using southernTravel.Model;

namespace southernTravel.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Orders>> GetAllAsync();
    }
}
