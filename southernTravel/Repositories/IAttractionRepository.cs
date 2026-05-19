using southernTravel.Model;

namespace southernTravel.Repositories
{
    public interface IAttractionRepository
    {   
        //角括號 < > 叫做「泛型（Generics）」，它允許我們在定義類別、方法或介面時使用一個占位符，這個占位符可以在實際使用時被具體的類型替換。在這裡，List<Attraction> 表示一個包含 Attraction 類型元素的列表。
        Task<List<Attraction>> GetAllAsync();

        Task<Attraction?> GetByIdAsync(int id);

        Task<Attraction> CreateAsync(Attraction attraction);

        Task UpdateAsync(Attraction attraction);

        Task DeleteAsync(Attraction attraction);
    }
}
