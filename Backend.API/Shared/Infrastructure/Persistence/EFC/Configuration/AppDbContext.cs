using Backend.API.IAM.Domain.Model.Aggregates;
using Backend.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Backend.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Backend.API.Groups.Domain.Model.Aggregates;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context for the Learning Center Platform
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Colleague> Colleagues { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Calculation> Calculations { get; set; }

   // ...existing code...
   /// <remarks>
   ///     This method is used to configure the database context.
   ///     It also adds the created and updated date interceptor to the database context.
   /// </remarks>
   /// <param name="builder">
   ///     The option builder for the database context
   /// </param>
   protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

   /// <summary>
   ///     On creating the database model
   /// </summary>
   /// <remarks>
   ///     This method is used to create the database model for the application.
   /// </remarks>
   /// <param name="builder">
   ///     The model builder for the database context
   /// </param>
   protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        //Bounded context builder:
        
        // IAM Context
        builder.ApplyIamConfiguration();
        
        // Groups Context
        builder.ApplyGroupsConfiguration();
        
        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}