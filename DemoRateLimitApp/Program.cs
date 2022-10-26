using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddOptions();

// needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();

//load general configuration from appsettings.json
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

//load ip rules from appsettings.json
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

// inject counter and rules stores
builder.Services.AddInMemoryRateLimiting();
//services.AddDistributedRateLimiting<AsyncKeyLockProcessingStrategy>();
//services.AddDistributedRateLimiting<RedisProcessingStrategy>();
//services.AddRedisRateLimiting();

// Add framework services.

// configuration (resolvers, counter key builders)
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.UseAuthorization();
app.MapControllers();
app.Run();
