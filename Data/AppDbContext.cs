using Microsoft.EntityFrameworkCore;
using FashionHub.Models;

namespace FashionHub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Data - Fashion Products with real Unsplash images
            modelBuilder.Entity<Product>().HasData(
                // Men's T-Shirts
                new Product { Id = 1, Name = "Classic White Oversized Tee", Description = "Premium cotton oversized fit t-shirt. Perfect for casual outings.", Price = 599, OriginalPrice = 1299, ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400&h=500&fit=crop", Category = "Men", Brand = "FashionHub Basics", Stock = 150, Rating = 4.5, ReviewCount = 2341, IsFeatured = true, Sizes = "S,M,L,XL,XXL", Colors = "White,Black,Grey" },
                new Product { Id = 2, Name = "Graphic Print Round Neck Tee", Description = "Trendy graphic print t-shirt with bold design.", Price = 449, OriginalPrice = 999, ImageUrl = "https://images.unsplash.com/photo-1503341504253-dff4815485f1?w=400&h=500&fit=crop", Category = "Men", Brand = "UrbanStyle", Stock = 200, Rating = 4.3, ReviewCount = 1856, IsFeatured = true, Sizes = "S,M,L,XL", Colors = "Black,Navy,Red" },
                new Product { Id = 3, Name = "Slim Fit Chino Pants", Description = "Smart casual chino pants. Perfect for office and outings.", Price = 1299, OriginalPrice = 2499, ImageUrl = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=400&h=500&fit=crop", Category = "Men", Brand = "TrendWear", Stock = 80, Rating = 4.6, ReviewCount = 987, IsFeatured = false, Sizes = "28,30,32,34,36", Colors = "Beige,Navy,Black,Olive" },
                new Product { Id = 4, Name = "Denim Jacket Regular Fit", Description = "Classic denim jacket with button closure. Timeless style.", Price = 1899, OriginalPrice = 3999, ImageUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=400&h=500&fit=crop", Category = "Men", Brand = "DenimCo", Stock = 60, Rating = 4.7, ReviewCount = 3421, IsFeatured = true, Sizes = "S,M,L,XL,XXL", Colors = "Blue,Black,Light Blue" },
                new Product { Id = 5, Name = "Formal Slim Fit Shirt", Description = "Premium cotton formal shirt for office wear.", Price = 899, OriginalPrice = 1799, ImageUrl = "https://images.unsplash.com/photo-1598033129183-c4f50c736f10?w=400&h=500&fit=crop", Category = "Men", Brand = "OfficeLook", Stock = 120, Rating = 4.4, ReviewCount = 1234, IsFeatured = false, Sizes = "S,M,L,XL,XXL", Colors = "White,Blue,Pink,Black" },

                // Women's Collection
                new Product { Id = 6, Name = "Floral Wrap Midi Dress", Description = "Beautiful floral print wrap dress. Perfect for all occasions.", Price = 1199, OriginalPrice = 2499, ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400&h=500&fit=crop", Category = "Women", Brand = "FloralByFH", Stock = 90, Rating = 4.8, ReviewCount = 4521, IsFeatured = true, Sizes = "XS,S,M,L,XL", Colors = "Red Floral,Blue Floral,Pink Floral" },
                new Product { Id = 7, Name = "High Waist Skinny Jeans", Description = "Ultra stretch high waist jeans for perfect fit and comfort.", Price = 1499, OriginalPrice = 2999, ImageUrl = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=400&h=500&fit=crop", Category = "Women", Brand = "DenimQueen", Stock = 110, Rating = 4.5, ReviewCount = 2876, IsFeatured = true, Sizes = "24,26,28,30,32", Colors = "Blue,Black,White,Grey" },
                new Product { Id = 8, Name = "Crop Top with Embroidery", Description = "Trendy embroidered crop top for casual and party wear.", Price = 699, OriginalPrice = 1499, ImageUrl = "https://images.unsplash.com/photo-1618354691373-d851c5c3a990?w=400&h=500&fit=crop", Category = "Women", Brand = "GirlBoss", Stock = 130, Rating = 4.3, ReviewCount = 1654, IsFeatured = false, Sizes = "XS,S,M,L", Colors = "White,Pink,Yellow,Black" },
                new Product { Id = 9, Name = "Ethnic Kurti with Palazzo", Description = "Traditional printed kurti with palazzo set. Festival ready.", Price = 1799, OriginalPrice = 3499, ImageUrl = "https://images.unsplash.com/photo-1583391733956-6c78276477e2?w=400&h=500&fit=crop", Category = "Women", Brand = "EthnicVibes", Stock = 75, Rating = 4.9, ReviewCount = 5632, IsFeatured = true, Sizes = "XS,S,M,L,XL,XXL", Colors = "Maroon,Green,Blue,Orange" },
                new Product { Id = 10, Name = "Blazer Women Formal", Description = "Professional women blazer. Sharp and sophisticated look.", Price = 2299, OriginalPrice = 4999, ImageUrl = "https://images.unsplash.com/photo-1594938298603-c8148c4b4fb8?w=400&h=500&fit=crop", Category = "Women", Brand = "PowerDress", Stock = 45, Rating = 4.6, ReviewCount = 876, IsFeatured = false, Sizes = "XS,S,M,L,XL", Colors = "Black,White,Navy,Camel" },

                // Kids Collection
                new Product { Id = 11, Name = "Kids Cartoon Printed Tee", Description = "Soft cotton cartoon tee for kids. Fun and comfortable.", Price = 349, OriginalPrice = 699, ImageUrl = "https://images.unsplash.com/photo-1519238263530-99bdd11df2ea?w=400&h=500&fit=crop", Category = "Kids", Brand = "KiddoWear", Stock = 200, Rating = 4.7, ReviewCount = 3210, IsFeatured = false, Sizes = "2Y,4Y,6Y,8Y,10Y,12Y", Colors = "Red,Blue,Yellow,Green" },
                new Product { Id = 12, Name = "Girls Frock Party Wear", Description = "Beautiful party frock for girls. Net fabric with bow.", Price = 899, OriginalPrice = 1799, ImageUrl = "https://images.unsplash.com/photo-1518831959646-742c3a14ebf7?w=400&h=500&fit=crop", Category = "Kids", Brand = "LittlePrincess", Stock = 85, Rating = 4.8, ReviewCount = 1432, IsFeatured = true, Sizes = "2Y,4Y,6Y,8Y,10Y", Colors = "Pink,Purple,White,Yellow" },

                // Footwear
                new Product { Id = 13, Name = "Men Running Sneakers", Description = "Lightweight running shoes with cushioned sole. High performance.", Price = 2499, OriginalPrice = 4999, ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&h=500&fit=crop", Category = "Footwear", Brand = "SpeedRun", Stock = 95, Rating = 4.6, ReviewCount = 4321, IsFeatured = true, Sizes = "6,7,8,9,10,11,12", Colors = "Black/White,Blue/White,Red/Black" },
                new Product { Id = 14, Name = "Women Heels Block", Description = "Comfortable block heels for work and casual wear.", Price = 1299, OriginalPrice = 2799, ImageUrl = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=400&h=500&fit=crop", Category = "Footwear", Brand = "StepUp", Stock = 70, Rating = 4.4, ReviewCount = 2109, IsFeatured = false, Sizes = "3,4,5,6,7,8", Colors = "Black,Nude,Red,White" },

                // Accessories
                new Product { Id = 15, Name = "Leather Tote Bag", Description = "Premium faux leather tote bag with spacious compartments.", Price = 1599, OriginalPrice = 3299, ImageUrl = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=400&h=500&fit=crop", Category = "Accessories", Brand = "BagsByFH", Stock = 55, Rating = 4.7, ReviewCount = 1876, IsFeatured = true, Sizes = "One Size", Colors = "Black,Brown,Tan,White" }
            );
        }
    }
}
