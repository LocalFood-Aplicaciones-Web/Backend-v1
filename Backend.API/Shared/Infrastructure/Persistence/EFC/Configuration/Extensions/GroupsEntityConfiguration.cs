using Backend.API.Groups.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class GroupsEntityConfiguration
{
    public static void ApplyGroupsConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Color).HasMaxLength(7);
            entity.Property(e => e.UserId).IsRequired();
        });

        modelBuilder.Entity<Colleague>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UserId).IsRequired();
            
            entity.OwnsOne(e => e.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.Street).HasMaxLength(255);
                addressBuilder.Property(a => a.City).HasMaxLength(100);
            });
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Cuisine).HasMaxLength(50);
            entity.Property(e => e.PriceRange).HasMaxLength(5);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.OpenHours).HasMaxLength(100);
            entity.Property(e => e.UserId).IsRequired();
            
            entity.OwnsOne(e => e.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.Street).HasMaxLength(255);
                addressBuilder.Property(a => a.City).HasMaxLength(100);
            });
        });

        modelBuilder.Entity<Calculation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GroupId).IsRequired();
            entity.Property(e => e.RestaurantId).IsRequired();
            entity.Property(e => e.RestaurantName).HasMaxLength(100);
            
            entity.OwnsMany(e => e.GroupMembers, colleagueBuilder =>
            {
                colleagueBuilder.ToJson();
            });
            
            entity.OwnsOne(e => e.CenterPoint, centerPointBuilder =>
            {
                centerPointBuilder.ToJson();
            });
            
            entity.OwnsMany(e => e.MembersByDistance, memberBuilder =>
            {
                memberBuilder.ToJson();
            });
        });
    }
}

