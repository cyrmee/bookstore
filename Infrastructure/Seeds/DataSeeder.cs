using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Seeds;

public static class DataSeeder
{
    public static async Task SeedBooks(IRepository _repository)
    {
        var books = new List<Book>()
        {
            new() {
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                Publisher = "Grand Central Publishing",
                PublicationDate = new DateTime(1960, 7, 11).ToUniversalTime(),
                Isbn10 = "0061120081",
                Isbn13 = "9780061120084",
                Description = "A gripping portrayal of racial injustice and the loss of innocence in the Deep South.",
                Quantity = 5,
                Price = 9.99
            },
            new() {
                Title = "1984",
                Author = "George Orwell",
                Publisher = "Signet Classics",
                PublicationDate = new DateTime(1949, 6, 8).ToUniversalTime(),
                Isbn10 = "0451524934",
                Isbn13 = "9780451524935",
                Description = "A dystopian novel depicting a totalitarian society and its impact on individuals' freedom.",
                Quantity = 8,
                Price = 12.99
            },
            new() {
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                Publisher = "Scribner",
                PublicationDate = new DateTime(1925, 4, 10).ToUniversalTime(),
                Isbn10 = "0743273567",
                Isbn13 = "9780743273565",
                Description = "A story of wealth, love, and tragedy set in the backdrop of the Jazz Age.",
                Quantity = 4,
                Price = 11.99
            },
            new() {
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                Publisher = "Penguin Classics",
                PublicationDate = new DateTime(1813, 1, 28).ToUniversalTime(),
                Isbn10 = "0141439513",
                Isbn13 = "9780141439518",
                Description = "A classic romance novel exploring themes of love, class, and societal expectations.",
                Quantity = 6,
                Price = 10.99
            },
            new() {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                Publisher = "Mariner Books",
                PublicationDate = new DateTime(1937, 9, 21).ToUniversalTime(),
                Isbn10 = "0618260307",
                Isbn13 = "9780618260300",
                Description = "An adventure novel set in Middle-earth, preceding the events of The Lord of the Rings.",
                Quantity = 10,
                Price = 13.99
            },
            new() {
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                Publisher = "Grand Central Publishing",
                PublicationDate = new DateTime(1960, 7, 11).ToUniversalTime(),
                Isbn10 = "0061120081",
                Isbn13 = "9780061120084",
                Description = "A gripping portrayal of racial injustice and the loss of innocence in the Deep South.",
                Quantity = 5,
                Price = 9.99
            },
            new() {
                Title = "Nineteen Eighty-Four",
                Author = "George Orwell",
                Publisher = "Penguin Classics",
                PublicationDate = new DateTime(1949, 6, 8).ToUniversalTime(),
                Isbn10 = "0141393049",
                Isbn13 = "9780141393049",
                Description = "A dystopian novel depicting a totalitarian society and its impact on individuals' freedom.",
                Quantity = 8,
                Price = 12.99
            },
            new() {
                Title = "The Catcher in the Rye",
                Author = "J.D. Salinger",
                Publisher = "Little, Brown and Company",
                PublicationDate = new DateTime(1951, 7, 16).ToUniversalTime(),
                Isbn10 = "0316769487",
                Isbn13 = "9780316769488",
                Description = "A coming-of-age novel exploring teenage rebellion, alienation, and identity.",
                Quantity = 7,
                Price = 11.99
            },
            new() {
                Title = "The Alchemist",
                Author = "Paulo Coelho",
                Publisher = "HarperOne",
                PublicationDate = new DateTime(1988, 4, 25).ToUniversalTime(),
                Isbn10 = "0062315005",
                Isbn13 = "9780062315007",
                Description = "A philosophical novel about following one's dreams and finding one's true purpose.",
                Quantity = 9,
                Price = 10.99
            },
            new() {
                Title = "Harry Potter and the Philosopher's Stone",
                Author = "J.K. Rowling",
                Publisher = "Bloomsbury Publishing",
                PublicationDate = new DateTime(1997, 6, 26).ToUniversalTime(),
                Isbn10 = "0747532699",
                Isbn13 = "9780747532699",
                Description = "The first book in the popular Harry Potter series, introducing the magical world of Hogwarts.",
                Quantity = 12,
                Price = 14.99
            }

        };

        foreach (var book in books)
        {
            if (await _repository.Book!.CountAsync() == 0)
                await _repository.Book.AddAsync(book);
        }

        await _repository.SaveAsync();
    }

    public static async Task SeedCategories(IRepository _repository)
    {
        var categories = new List<Category>()
        {
            new() { Name = "Fiction" },
            new() { Name = "Science Fiction" },
            new() { Name = "Fantasy" },
            new() { Name = "Mystery" },
            new() { Name = "Thriller" },
            new() { Name = "Romance" },
            new() { Name = "Biography" },
            new() { Name = "History" },
            new() { Name = "Cooking" },
            new() { Name = "Science" },
            new() { Name = "Technology" },
            new() { Name = "Business" },
            new() { Name = "Health" },
            new() { Name = "Fitness" },
            new() { Name = "Travel" },
            new() { Name = "Art" },
            new() { Name = "Music" },
            new() { Name = "Sports" },
            new() { Name = "Religion" },
            new() { Name = "Children" },
            new() { Name = "Comics" },
            new() { Name = "Graphic Novels" },
            new() { Name = "Poetry" },
            new() { Name = "Drama" },
            new() { Name = "Classics" }
        };

        foreach (var category in categories)
        {
            if (await _repository.Category!.CountAsync() == 0)
            {
                await _repository.Category.AddAsync(category);
            }
        }

        await _repository.SaveAsync();
    }
}
