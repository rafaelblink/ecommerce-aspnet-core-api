using Microsoft.EntityFrameworkCore;

namespace HPlusSport.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Active Wear - Men" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Skateboard",
                    Sku = "SK8",
                    Description = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry. 
                    Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
                    Price = 1500.22M,
                    isAvailable = true,
                    CategoryId = 1
                }
            );
        }
    }
}