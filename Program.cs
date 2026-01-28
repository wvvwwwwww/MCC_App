using MccApi.Data;
using MccApi.Mapping;
using MccApi.Repositories.Implementations;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Добавляем DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITitleRepository, TitleRepository>();
builder.Services.AddScoped<IPointRepository, PointRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IEmployeeScheduleRepository, EmployeeScheduleRepository>();
builder.Services.AddScoped<IChangeRepository, ChangeRepository>();
builder.Services.AddScoped<IChangeStatusRepository, ChangeStatusRepository>();
builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
builder.Services.AddScoped<IMeetingTopicRepository, MeetingTopicRepository>();
builder.Services.AddScoped<IMeetingStatusRepository, MeetingStatusRepository>();
builder.Services.AddScoped<IChangesHistoryRepository, ChangesHistoryRepository>();
builder.Services.AddScoped<IWeekDayRepository, WeekDayRepository>();
builder.Services.AddScoped<IAutorizationRepository, AutorizationRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();

// AutoMapper - ВРЕМЕННО отключён
// builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
 builder.Services.AddAutoMapper(typeof(Program));

// Контроллеры
builder.Services.AddControllers();
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]
    ?? "your-secret-key-min-32-chars-long-1234567890");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "MccApi",
        ValidAudience = jwtSettings["Audience"] ?? "MccClient",
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWpfClient",
       policy =>
       {
           policy.AllowAnyOrigin()   // Разрешаем все origin (для разработки)
                 .AllowAnyMethod()
                 .AllowAnyHeader();
       });
});

var app = builder.Build();

// Конвейер обработки запросов
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();