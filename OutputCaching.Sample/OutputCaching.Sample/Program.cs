using OutputCaching.Sample.Endpoints;
using OutputCaching.Sample.ExternalStore;
using OutputCaching.Sample.Service;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProductService,ProductService>();

//REDIS CACHE
//builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("localhost:6379,password=mylocalredispassword"));
//builder.Services.AddRedisOutputCache();

//IN-MEMORY BUILT-IN CACHE
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(x => x.NoCache());

    options.AddPolicy("with_cache", x =>
    {
        x.Cache();
        x.Expire(TimeSpan.FromSeconds(10));
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();
app.MapProductEndpoint();

app.Run();


