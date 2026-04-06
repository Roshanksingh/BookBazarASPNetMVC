using BookBazar.Models;
using Microsoft.EntityFrameworkCore;

namespace BookBazar.DataAccess.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Self Help", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Biography", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Fiction", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Historical Fiction", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Thriller", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Fantasy", DisplayOrder = 6 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Surrounded by Idiots",
                    Author = "Thomas Erikson",
                    Description = "A popular self-help book about understanding personality types and improving communication with different kinds of people.",
                    ISBN = "9781250179944",
                    ListPrice = 18,
                    Price = 16,
                    Price50 = 14,
                    Price100 = 12,
                    CategoryId = 1,
                    ImageUrl = "/images/products/books/surrounded-by-idiots.jpg"
                },

                new Product
                {
                    Id = 2,
                    Title = "Becoming",
                    Author = "Michelle Obama",
                    Description = "A memoir by Michelle Obama about her life, family, values, and journey from childhood to the White House.",
                    ISBN = "9781524763138",
                    ListPrice = 20,
                    Price = 18,
                    Price50 = 16,
                    Price100 = 14,
                    CategoryId = 2,
                    ImageUrl = "/images/products/books/becoming.jpg"
                },

                new Product
                {
                    Id = 3,
                    Title = "Ikigai",
                    Author = "Héctor García and Francesc Miralles",
                    Description = "A self-help book exploring the Japanese idea of ikigai, or finding purpose, meaning, and joy in life.",
                    ISBN = "9780143130727",
                    ListPrice = 17,
                    Price = 15,
                    Price50 = 13,
                    Price100 = 11,
                    CategoryId = 1,
                    ImageUrl = "/images/products/books/ikigai.jpg"
                },

                new Product
                {
                    Id = 4,
                    Title = "Atomic Habits",
                    Author = "James Clear",
                    Description = "A bestselling self-improvement book about building good habits, breaking bad ones, and making small changes that produce big results.",
                    ISBN = "9780735211292",
                    ListPrice = 20,
                    Price = 18,
                    Price50 = 16,
                    Price100 = 14,
                    CategoryId = 1,
                    ImageUrl = "/images/products/books/atomic-habits.jpg"
                },

                new Product
                {
                    Id = 5,
                    Title = "The Correspondent",
                    Author = "Virginia Evans",
                    Description = "A literary fiction novel centered on relationships, emotional depth, and human connection.",
                    ISBN = "9780593654477",
                    ListPrice = 19,
                    Price = 17,
                    Price50 = 15,
                    Price100 = 13,
                    CategoryId = 3,
                    ImageUrl = "/images/products/books/the-correspondent.jpg"
                },

                new Product
                {
                    Id = 6,
                    Title = "Theo of Golden",
                    Author = "Allen Levi",
                    Description = "A reflective and emotional novel about life, memory, and meaningful human experiences.",
                    ISBN = "9781637632697",
                    ListPrice = 18,
                    Price = 16,
                    Price50 = 14,
                    Price100 = 12,
                    CategoryId = 3,
                    ImageUrl = "/images/products/books/theo-of-golden.jpg"
                },

                new Product
                {
                    Id = 7,
                    Title = "Believe in Yourself",
                    Author = "Joseph Murphy",
                    Description = "A motivational self-help classic focused on confidence, belief, and the power of the mind.",
                    ISBN = "9789355994653",
                    ListPrice = 15,
                    Price = 13,
                    Price50 = 11,
                    Price100 = 9,
                    CategoryId = 1,
                    ImageUrl = "/images/products/books/believe-in-yourself.jpg"
                },

                new Product
                {
                    Id = 8,
                    Title = "All the Light We Cannot See",
                    Author = "Anthony Doerr",
                    Description = "A bestselling historical fiction novel set during World War II, following the lives of a blind French girl and a German boy.",
                    ISBN = "9781501173219",
                    ListPrice = 19,
                    Price = 17,
                    Price50 = 15,
                    Price100 = 13,
                    CategoryId = 4,
                    ImageUrl = "/images/products/books/all-the-light-we-cannot-see.jpg"
                },

                new Product
                {
                    Id = 9,
                    Title = "A Flicker in the Dark",
                    Author = "Stacy Willingham",
                    Description = "A psychological thriller with suspense, secrets, and disturbing twists tied to crimes from the past.",
                    ISBN = "9781250803825",
                    ListPrice = 18,
                    Price = 16,
                    Price50 = 14,
                    Price100 = 12,
                    CategoryId = 5,
                    ImageUrl = "/images/products/books/a-flicker-in-the-dark.jpg"
                },

                new Product
                {
                    Id = 10,
                    Title = "You Become What You Think",
                    Author = "Shubham Kumar Singh",
                    Description = "A motivational and mindset-focused self-help book about mastering thoughts and shaping your life through positive thinking.",
                    ISBN = "9780143475279",
                    ListPrice = 16,
                    Price = 14,
                    Price50 = 12,
                    Price100 = 10,
                    CategoryId = 1,
                    ImageUrl = "/images/products/books/you-become-what-you-think.jpg"
                },

                new Product
                {
                    Id = 11,
                    Title = "The Psychology of Money",
                    Author = "Morgan Housel",
                    Description = "A personal finance and self-development book explaining how behavior, emotions, and decision-making shape financial success.",
                    ISBN = "9780857197689",
                    ListPrice = 18,
                    Price = 16,
                    Price50 = 14,
                    Price100 = 12,
                    CategoryId = 1,
                    ImageUrl = "/images/products/books/the-psychology-of-money.jpg"
                },

                new Product
                {
                    Id = 12,
                    Title = "Fourth Wing",
                    Author = "Rebecca Yarros",
                    Description = "A fantasy novel filled with dragons, war college training, danger, and romance.",
                    ISBN = "9781649374042",
                    ListPrice = 22,
                    Price = 20,
                    Price50 = 18,
                    Price100 = 16,
                    CategoryId = 6,
                    ImageUrl = "/images/products/books/fourth-wing.jpg"
                },

                new Product
                {
                    Id = 13,
                    Title = "The Let Them Theory",
                    Author = "Mel Robbins",
                    Description = "A self-help and mindset book focused on letting go of control, reducing stress, and living with more freedom.",
                    ISBN = "9781401971366",
                    ListPrice = 19,
                    Price = 17,
                    Price50 = 15,
                    Price100 = 13,
                    CategoryId = 1,
                    ImageUrl = "/images/products/books/the-let-them-theory.jpg"
                }
            );
        }
    }
}
