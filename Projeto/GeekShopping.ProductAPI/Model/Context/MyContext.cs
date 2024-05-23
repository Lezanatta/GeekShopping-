using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Model.Context;
public class MyContext : IdentityDbContext<ApplicationUser>
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 7,
            Name = "Moletom cobra kay",
            CategoryName = "T-shirt",
            Description = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde",
            ImageUrl = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/8_moletom_cobra_kay.jpg",
            Price = new decimal(169.9)
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 8,
            Name = "Brinquedo milenium falcon",
            CategoryName = "Brinquedo",
            Description = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde",
            ImageUrl = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/10_milennium_falcon.jpg",
            Price = new decimal(49.9)
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 9,
            Name = "Camiseta dragon ball",
            CategoryName = "T-shirt",
            Description = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde",
            ImageUrl = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/13_dragon_ball.jpg",
            Price = new decimal(89.9)
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 10,
            Name = "Livro neil",
            CategoryName = "Livro",
            Description = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde",
            ImageUrl = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/9_neil.jpg",
            Price = new decimal(69.9)
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 11,
            Name = "Capacete date vader",
            CategoryName = "Brinquedo",
            Description = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde",
            ImageUrl = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/3_vader.jpg",
            Price = new decimal(39.9)
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 12,
            Name = "Brinquedo tropper",
            CategoryName = "T-shirt",
            Description = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde",
            ImageUrl = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/4_storm_tropper.jpg",
            Price = new decimal(59.9)
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 13,
            Name = "Camiseta gamer",
            CategoryName = "T-shirt",
            Description = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde",
            ImageUrl = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/5_100_gamer.jpg",
            Price = new decimal(49.9)
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 14,
            Name = "Camiseta coffe",
            CategoryName = "T-shirt",
            Description = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde",
            ImageUrl = "https://github.com/leandrocgsi/erudio-microservices-dotnet6/blob/main/ShoppingImages/7_coffee.jpg",
            Price = new decimal(79.9)
        });

        base.OnModelCreating(modelBuilder);
    }
}
