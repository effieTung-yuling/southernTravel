namespace southernTravel.DTOs
{
    // DTO = Data Transfer Object
    // 中文：資料傳輸物件
    // 主要用途：
    // 「控制 API 要傳什麼資料」
    //
    // DTO 不代表資料庫，
    // 而是代表：
    // API 要給前端的資料格式
    //
    // 一個 Entity 可以有很多 DTO
    // 因為：
    // 不同 API、不同畫面，需要的資料不同

    // =========================================================
    // OrdersDto（列表 DTO）
    // =========================================================
    //
    // 用途：
    // 給「列表頁」使用
    //
    // 例如：
    // 訂單列表
    //
    // | 訂單編號 | 名稱 |
    //
    // 因為列表通常只需要簡單資訊，
    // 所以不需要 CreatedAt、UpdatedAt
    //
    // 好處：
    // 1. 減少 API 傳輸資料量
    // 2. 不回傳不必要欄位
    // 3. 前端畫面更單純
    public class OrdersDto
    {
        // 訂單編號
        public string OrderNo { get; set; } = string.Empty;

        // 訂單名稱
        public string Name { get; set; } = string.Empty;
    }

    // =========================================================
    // OrderDetailDto（詳細 DTO）
    // =========================================================
    //
    // 用途：
    // 給「詳細頁」使用
    //
    // 例如：
    // 點進訂單詳細資料頁面
    //
    // 詳細頁通常需要更多資訊，
    // 所以這裡多了：
    // CreatedAt
    //
    // 注意：
    // DTO 不一定都一樣，
    // 要依照「畫面需求」設計
    public class OrderDetailDto
    {
        // 訂單編號
        public string OrderNo { get; set; } = string.Empty;

        // 訂單名稱
        public string Name { get; set; } = string.Empty;

        // 建立時間
        // 詳細頁才需要顯示
        public DateTime CreatedAt { get; set; }
    }

    // =========================================================
    // CreateOrderDto（新增 DTO）
    // =========================================================
    //
    // 用途：
    // 新增訂單 API 使用
    //
    // 例如：
    // POST /api/orders
    //
    // 前端新增資料時，
    // 通常只需要輸入：
    // 1. OrderNo
    // 2. Name
    //
    // 不需要：
    // 1. Id
    // 2. CreatedAt
    // 3. UpdatedAt
    //
    // 因為：
    // 這些通常由後端自己產生
    //
    // 這樣做可以避免：
    // 前端亂傳不該修改的欄位
    public class CreateOrderDto
    {
        // 訂單編號
        public string OrderNo { get; set; } = string.Empty;

        // 訂單名稱
        public string Name { get; set; } = string.Empty;
    }
}
