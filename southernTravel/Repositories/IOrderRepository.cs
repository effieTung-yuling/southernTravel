using southernTravel.Model;

namespace southernTravel.Repositories
{
    // Repository 介面
    // 定義「有哪些資料庫操作功能」
    public interface IOrderRepository
    {
        // 取得全部 Orders 資料
        // Task = 非同步
        // List<Orders> = 回傳多筆 Orders 資料
        Task<List<Orders>> GetAllAsync();
    }
}