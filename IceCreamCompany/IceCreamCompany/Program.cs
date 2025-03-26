using IceCreamCompany.Api.Extensions;
using IceCreamCompany.Application.Core.Interfaces;
using IceCreamCompany.Application.Core.Services;
using IceCreamCompany.Domain.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterAutoMapper();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString(GlobalConstants.RedisConnectionStringName);
    options.InstanceName = GlobalConstants.RedisInstanceName;
});

builder.Services.AddHttpClient<IUniversalLoaderService, UniversalLoaderService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration[GlobalConstants.BaseUrl]);
});

builder.Services.AddCustomCors(builder.Configuration);

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.RegisterDbContext(builder.Configuration);

builder.Services.AddCustomSwaggerExtension();

builder.Services.RegisterQuartzJobs();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();