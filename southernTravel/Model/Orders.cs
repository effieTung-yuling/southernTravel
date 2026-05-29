namespace southernTravel.Model { 
    // Orders 代表「訂單資料表」對應的 Entity Model 
    // Entity Framework Core 之後會透過這個 class 去 mapping 資料庫 table
    public class Orders { 
        // 主鍵（Primary Key） 
        // EF Core 看到 Id 通常會自動判斷為 PK 
        // long 對應 SQL Server 常見的 bigint
        public long Id { get; set; }
        // 訂單編號 // = string.Empty 是避免 nullable 警告 
        // 如果沒給預設值，C# 可能會警告： 
        // Non-nullable property must contain a non-null value
        public string OrderNo { get; set; } = string.Empty;
        // 訂單名稱 / 使用者名稱 / 商品名稱 
        // 之後可依需求調整欄位用途
        public string Name { get; set; } = string.Empty; 
        // 建立時間 // 通常新增資料時設定： 
        // DateTime.UtcNow 或 DateTime.Now
        public DateTime CreatedAt { get; set; }
        // 更新時間 
        // 通常修改資料時更新
        public DateTime UpdatedAt { get; set; }
    } 
}