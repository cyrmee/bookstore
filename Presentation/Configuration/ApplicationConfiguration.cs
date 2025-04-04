﻿using Application.Dtos;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Interceptors;
using Infrastructure.Repositories.Implementation;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

namespace Presentation.Configuration;

public abstract class ApplicationConfiguration
{
    public static void Configure(WebApplicationBuilder builder)
    {
        ConfigureGeneralAppServices(builder);
        ConfigureAuthenticationServices(builder);
        ConfigureApplicationServices(builder);
        ConfigureRepositories(builder);
        ConfigureDatabaseServices(builder);
        ConfigureAutoMapper(builder);
        ConfigureSerilogServices(builder);
    }

    private static void ConfigureGeneralAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddMemoryCache();

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    private static void ConfigureAuthenticationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<BookStoreDbContext>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtBearer:Issuer"],
                    ValidAudience = builder.Configuration["JwtBearer:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        builder.Services.AddAuthorization();
    }

    private static void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IBookCategoryService, BookCategoryService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    private static void ConfigureRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRepository, Repository>();
    }

    private static void ConfigureDatabaseServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        builder.Services.AddScoped<SoftDeleteInterceptor>();

        builder.Services.AddEntityFrameworkNpgsql()
            .AddDbContext<BookStoreDbContext>((sp, options) =>
            {
                options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseInternalServiceProvider(sp);
            });

        ApplyDatabaseMigrations(builder);
        EnsureDataPopulated(builder);
    }

    private static void ApplyDatabaseMigrations(WebApplicationBuilder builder)
    {
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var _bookStoreDbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

        _bookStoreDbContext.Database.Migrate();
    }

    private static void EnsureDataPopulated(WebApplicationBuilder builder)
    {
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var _repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        DataSeeder.SeedBooks(_repository).Wait();
        DataSeeder.SeedCategories(_repository).Wait();
        IdentitySeeder.SeedRoles(_roleManager).Wait();
        IdentitySeeder.SeedUsers(_userManager).Wait();
    }

    // private static void ConfigureRedisCache(WebApplicationBuilder builder)
    // {
    // 	builder.Services.AddStackExchangeRedisCache(options =>
    // 	{
    // 		options.Configuration = builder.Configuration["RedisCache:ConnectionString"];
    // 		options.InstanceName = builder.Configuration["RedisCache:InstanceName"];
    // 	});
    // }

    private static void ConfigureAutoMapper(WebApplicationBuilder builder) =>
        builder.Services.AddSingleton(_ => new MapperConfiguration(m =>
        {
            m.CreateMap<Book, BookDto>().ReverseMap();
            m.CreateMap<Book, BookWriteDto>().ReverseMap();

            m.CreateMap<BookCategory, BookCategoryDto>().ReverseMap();
            m.CreateMap<BookCategory, BookCategoryWriteDto>().ReverseMap();

            m.CreateMap<Category, CategoryDto>().ReverseMap();
            m.CreateMap<Category, CategoryWriteDto>().ReverseMap();

            m.CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            m.CreateMap<OrderDetail, OrderDetailWriteDto>().ReverseMap();

            m.CreateMap<Order, OrderDto>().ReverseMap();
            m.CreateMap<Order, OrderWriteDto>().ReverseMap();

            m.CreateMap<User, UserSignupDto>().ReverseMap();
            m.CreateMap<User, UserInfoDto>().ReverseMap();
        }).CreateMapper());

    private static void ConfigureSerilogServices(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning).Enrich
            .FromLogContext().Enrich
            .WithProperty("OS", Environment.OSVersion).Enrich
            .WithProperty("MachineName", Environment.MachineName).Enrich
            .WithProperty("ProcessId", Environment.ProcessId).Enrich
            .WithProperty("ThreadId", Environment.CurrentManagedThreadId).WriteTo
            .Console().WriteTo
            .PostgreSQL(builder.Configuration["ConnectionStrings:DefaultConnection"], "Logs", needAutoCreateTable: true,
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }
}