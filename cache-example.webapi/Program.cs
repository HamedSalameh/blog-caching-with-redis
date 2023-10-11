using cache_example.repository;
using cache_example.webapi.Cache;
using cache_example.webapi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    // setup redis distributed cache
    options.InstanceName = "redis";
    options.Configuration = builder.Configuration.GetConnectionString("redisConnectionString");
});

builder.Services.AddSingleton<IRedisDbProvider>(provider =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
    return new RedisDbProvider(redisConnectionString);
});
builder.Services.AddSingleton<ICacheHandler, RedisCacheHandler>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add In-memory ef core database
builder.Services.AddDbContext<ProductsRepository>(options =>
{
    options.UseInMemoryDatabase("CacheExampleDb");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
