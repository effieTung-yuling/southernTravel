using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using southernTravel.Data;
using southernTravel.Repositories;
using southernTravel.Services;
using southernTravel.Validators;
using System.Text;

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

// JWT 驗證
var jwtKey = builder.Configuration["JwtSettings:JWT_KEY"]
             ?? builder.Configuration["JWT_KEY"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new Exception("未設定 JWT_KEY，請檢查 appsettings.json 中的 JwtSettings 區塊");
}

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
if (keyBytes.Length < 32)
{
    throw new Exception("JWT_KEY 長度不足，至少需要 32 bytes");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { message = "未授權，請先登入或提供有效的 Token" });
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { message = "權限不足，無法存取此資源" });
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IAttractionRepository, AttractionRepository>();
builder.Services.AddScoped<IAttractionService, AttractionService>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(er => er.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            message = "資料驗證失敗",
            errors
        });
    };
});

builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// 註冊 Controller
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // 忽略循環引用，避免直接當機
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddOpenApi();

var app = builder.Build();

// --- 順序 1：CORS ---
app.UseCors("AllowAll");

// --- 順序 2：API 文件 ---
app.MapOpenApi();
app.MapScalarApiReference(options => {
    options.WithTitle("southernTravel")
           .WithTheme(ScalarTheme.Moon);
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

////自動建立資料表
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // 加一行測試連線
        var canConnect = context.Database.CanConnect();
        if (canConnect)
        {
            context.Database.EnsureCreated();
            Console.WriteLine("✅ 資料庫連線與初始化成功！");
        }
        else
        {
            Console.WriteLine("❌ 無法連線到資料庫，請檢查連線字串。");
        }
    }
    catch (Exception ex)
    {
        // 這一行非常重要，會告訴我們具體錯在哪
        Console.WriteLine($"🔥 啟動錯誤: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"🔥 詳細原因: {ex.InnerException.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.Run(); // 本機會自動抓 launchSettings.json 裡的 Port (通常是 5xxx)
}
else
{
    app.Run("http://0.0.0.0:8080"); // 雲端用 8080
}