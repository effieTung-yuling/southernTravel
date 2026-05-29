using Microsoft.EntityFrameworkCore;
using southernTravel.Data;
using southernTravel.Model;
using southernTravel.Repositories;

// Repository = 資料存取層
// 專門負責跟資料庫溝通
public class OrderRepository : IOrderRepository
{
    // 注入資料庫 Context
    // 之後可以透過 _context 操作資料表
    private readonly AppDbContext _context;

    // 建構式注入 Dependency Injection (DI)
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    // 取得所有訂單資料
    // async 非同步方法
    // Task<List<Orders>> = 回傳 Orders 集合
    public async Task<List<Orders>> GetAllAsync()
    {
        // _context.Orders
        // 對應資料庫 Orders 資料表

        // ToListAsync()
        // 查詢資料並轉成 List
        return await _context.Orders.ToListAsync();
    }
}