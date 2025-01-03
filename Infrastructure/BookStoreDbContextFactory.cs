using Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

// public class BookStoreDbContextFactory : IDesignTimeDbContextFactory<BookStoreDbContext>
// {
//     public BookStoreDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<BookStoreDbContext>();

//          IConfigurationRoot configuration = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json")
//             .Build();

//         optionsBuilder.UseNpgsql(configuration.GetConnectionString("ConnectionStrings:DefaultConnection"));

//         return new BookStoreDbContext(optionsBuilder.Options, new AuditableEntitySaveChangesInterceptor(), new SoftDeleteInterceptor());
//     }
// }
