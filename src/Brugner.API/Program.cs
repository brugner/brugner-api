using AspNetCoreRateLimit;
using Brugner.API.Core.Extensions;
using Brugner.API.Core.Options;
using Brugner.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string cstr = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<BrugnerDbContext>(options => options.UseSqlServer(cstr));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder
                .WithOrigins("http://localhost:4200", "https://localhost:4200", "https://brugner.azurewebsites.net")
                .AllowAnyHeader()
                .AllowAnyMethod()
    );
});

builder.Services.AddControllers();
builder.Services.AddSwagger();

builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtOptions>(jwtSection);
builder.Services.AddJwtAuthentication(jwtSection.Get<JwtOptions>());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApplicationServices();

// Configure the HTTP request pipeline.
var app = builder.Build();

app.UpdateDatabase(app.Environment);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error-dev");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
