using southernTravel.Data;
using southernTravel.Repositories;
using southernTravel.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. 註冊 CORS 服務
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 取得連線字串
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // 如果抓不到連線字串（代表是在本機），才用 SQLite
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=dev.db"));
}
else
{
    // 只要有連線字串（代表在 Zeabur 且變數設定正確），就用 PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
}
builder.Services.AddScoped<MembersRepositories>();
builder.Services.AddScoped<MembersServices>();
builder.Services.AddScoped<ProductsRepositories>();
builder.Services.AddScoped<ProductsServices>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// --- 順序 1：CORS ---
app.UseCors("AllowAll");

// --- 順序 2：API 文件 ---
app.MapOpenApi();
app.MapScalarApiReference(options => {
    options.WithTitle("我的 API 文件")
           .WithTheme(ScalarTheme.Moon);
});

app.UseAuthorization();
app.MapControllers();

////自動建立資料表
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        var context = services.GetRequiredService<AppDbContext>();
//        // 加一行測試連線
//        var canConnect = context.Database.CanConnect();
//        if (canConnect)
//        {
//            context.Database.EnsureCreated();
//            Console.WriteLine("✅ 資料庫連線與初始化成功！");
//        }
//        else
//        {
//            Console.WriteLine("❌ 無法連線到資料庫，請檢查連線字串。");
//        }
//    }
//    catch (Exception ex)
//    {
//        // 這一行非常重要，會告訴我們具體錯在哪
//        Console.WriteLine($"🔥 啟動錯誤: {ex.Message}");
//        if (ex.InnerException != null)
//            Console.WriteLine($"🔥 詳細原因: {ex.InnerException.Message}");
//    }
//}

if (app.Environment.IsDevelopment())
{
    app.Run(); // 本機會自動抓 launchSettings.json 裡的 Port (通常是 5xxx)
}
else
{
    app.Run("http://0.0.0.0:8080"); // 雲端用 8080
}