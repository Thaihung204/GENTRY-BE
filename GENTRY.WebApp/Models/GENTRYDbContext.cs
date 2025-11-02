using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GENTRY.WebApp.Models;

public partial class GENTRYDbContext : DbContext
{
    public GENTRYDbContext()
    {
    }

    public GENTRYDbContext(DbContextOptions<GENTRYDbContext> options) : base(options) 
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
        return config["ConnectionStrings:DefaultConnectionString"];
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Style> Styles { get; set; }
    public virtual DbSet<SizeChart> SizeCharts { get; set; }
    public virtual DbSet<Partner> Partners { get; set; }
    public virtual DbSet<OutfitItem> OutfitItems { get; set; }
    public virtual DbSet<OutfitFeatureValue> OutfitFeatureValues { get; set; }
    public virtual DbSet<OutfitFeature> OutfitFeatures { get; set; }
    public virtual DbSet<Outfit> Outfits { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<Feature> Features { get; set; }
    public virtual DbSet<Color> Colors { get; set; }
    public virtual DbSet<CollectionItem> CollectionItems { get; set; }
    public virtual DbSet<Collection> Collections { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Analysis> Analysis { get; set; }
    public virtual DbSet<AiTrainingData> AITrainingData { get; set; }
    public virtual DbSet<AIModelVersion> AIModelVersions { get; set; }
    public virtual DbSet<AffiliateLink> AffiliateLinks { get; set; }
    public virtual DbSet<Admin> Admins { get; set; }
    public virtual DbSet<Weather> Weathers { get; set; }
    public virtual DbSet<Occasion> Occasions { get; set; }
    public virtual DbSet<AIChatMessage> AIChatMessages { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



        modelBuilder.Entity<AiTrainingData>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Quan hệ với Outfit
            entity.HasOne(e => e.Outfit)
                  .WithMany(o => o.AiTrainingData) // nhớ thêm ICollection<AiTrainingData> trong Outfit
                  .HasForeignKey(e => e.OutfitId)
                  .OnDelete(DeleteBehavior.Cascade); // vẫn cho phép xóa Outfit thì xóa dữ liệu liên quan

            // Quan hệ với User
            entity.HasOne(e => e.User)
                  .WithMany(u => u.AiTrainingData) // nhớ thêm ICollection<AiTrainingData> trong User
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // tránh multiple cascade path
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasOne(c => c.User)
                  .WithMany(u => u.Collections)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // hoặc NoAction
        });

        // Cấu hình Feedback relationship với User (optional)
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasOne(f => f.User)
                  .WithMany() // Feedback không có collection trong User, chỉ là optional relationship
                  .HasForeignKey(f => f.UserId)
                  .OnDelete(DeleteBehavior.SetNull); // Nếu user bị xóa, set UserId = null
        });
    }

}
