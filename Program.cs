using System.Reflection;
using FrightNight.Data;
using FrightNight.Repositories;
using FrightNight.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Session;

var builder = WebApplication.CreateBuilder(args);

// 設定 CORS 策略
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNgrok",
        policy =>
        {
            policy.AllowAnyOrigin()   // 或者 .WithOrigins("https://dafb-36-227-128-165.ngrok-free.app")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // 使用 PostgreSQL

builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IMemoryService, MemoryService>();
builder.Services.AddScoped<IMemoryRepository, MemoryRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddControllers();

// 添加 Session 服務
builder.Services.AddDistributedMemoryCache(); // 添加內存快取
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 設定 Session 超時
    options.Cookie.HttpOnly = true; // 使 Cookie 只能由伺服器訪問
    options.Cookie.IsEssential = true; // 使其成為必需的 Cookie
});

// JWT 認證配置
var key = builder.Configuration["Jwt:Key"]; // 從配置中讀取密鑰
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // 開發環境中可以設為 false
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FrightNightAPI",
        Description = "Fright Day Night API ",
        Contact = new OpenApiContact
        {
            Name = "Jerry Chen",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    // 讀取 XML 檔案產生 API 說明
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
app.UseCors("AllowNgrok");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FrightNightAPI V1");
    });
}

app.UseHttpsRedirection();
app.UseSession(); 
app.UseCors("AllowNgrok"); // Apply CORS policy
// 使用 JWT 認證
app.UseAuthentication(); // 確保在 UseAuthorization 之前調用
app.UseAuthorization();

app.MapControllers();

app.Run();

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // 使用 PostgreSQL
// // Add services to the container.

// builder.Services.AddScoped<ITodoRepository, TodoRepository>();
// builder.Services.AddScoped<ITodoService, TodoService>();
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();



// builder.Services.AddSwaggerGen(options =>
// {
//     options.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Version = "v1",
//         Title = "FrightNightAPI",
//         Description = "Fright Day Night API ",
//         // TermsOfService = new Uri("https://example.com/terms"),
//         Contact = new OpenApiContact
//         {
//             Name = "Jerry Chen",
//             Url = new Uri("https://example.com/contact")
//         },
//         License = new OpenApiLicense
//         {
//             Name = "Example License",
//             Url = new Uri("https://example.com/license")
//         }
//     });

//     // 讀取 XML 檔案產生 API 說明
//     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//     options.IncludeXmlComments(xmlPath);
// });

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "FrightNightAPI V1");
//     });
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();
