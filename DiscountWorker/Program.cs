
using Microsoft.EntityFrameworkCore;
using DiscountWorker.Domain.Interfaces;
using DiscountWorker.Infrastructure.Data;
using DiscountWorker.Infrastructure.Network;
using DiscountWorker.Application.Services;
using DiscountWorker.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Design;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var config = hostContext.Configuration;

        // Configure EF Core with SQL Server
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // Register DI services
        services.AddScoped<IMessageProcessor, MessageProcessor>();
        services.AddScoped<IDiscountCodeService, DiscountCodeService>();
        services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();

        // Register TCP Worker
        services.AddHostedService<TcpServerService>();

        // Optional: Initialize the database on startup
        services.AddHostedService<DbInitializerWorker>();
    })
    .Build();

await host.RunAsync();
