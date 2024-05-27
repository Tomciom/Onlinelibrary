namespace Onlinelibrary.Data;

using System.Security.Cryptography;
using System.Text;
using Onlinelibrary.Models;

public static class DbInitializer
{
    public static void Initialize(LibraryContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any())
        {
            return;   // DB has been seeded
        }

        var authors = new Author[]
        {
            new Author { Name = "George R. R. Martin" },
            new Author { Name = "J. K. Rowling" },
            new Author { Name = "J. R. R. Tolkien" }
        };

        foreach (var a in authors)
        {
            context.Authors.Add(a);
        }

        context.SaveChanges();

        var books = new Book[]
        {
            new Book { Title = "A Game of Thrones", Genre = "Fantasy", AuthorId = authors[0].Id },
            new Book { Title = "Harry Potter and the Philosopher's Stone", Genre = "Fantasy", AuthorId = authors[1].Id },
            new Book { Title = "The Fellowship of the Ring", Genre = "Fantasy", AuthorId = authors[2].Id }
        };

        foreach (var b in books)
        {
            context.Books.Add(b);
        }

        context.SaveChanges();

        var admin = new User 
        {
            Username = "admin",
            PasswordHash = ComputeHash("admin"),
            isAdmin = true
        };

        context.Users.Add(admin);

        context.SaveChanges();

        var users = new User[]
        {
            new User { Username = "John", PasswordHash = ComputeHash("123"), isAdmin = false},
            new User { Username = "Jane", PasswordHash = ComputeHash("456"), isAdmin = false},
            new User { Username = "Bob", PasswordHash = ComputeHash("789"), isAdmin = false}
        };

        foreach (var u in users)
        {
            context.Users.Add(u);
        }

        context.SaveChanges();

        var reviews = new Review[]
        {
            new Review { BookId = books[0].Id, Rating = 5, Content = "Great book!", UserId = users[0].Id},
            new Review { BookId = books[1].Id, Rating = 4, Content = "I enjoyed it!", UserId = users[0].Id},
            new Review { BookId = books[2].Id, Rating = 5, Content = "Amazing!", UserId = users[0].Id},
            new Review { BookId = books[0].Id, Rating = 4, Content = "Totally recommend reading", UserId = users[1].Id},
            new Review { BookId = books[1].Id, Rating = 2, Content = "Boooring!", UserId = users[1].Id},
            new Review { BookId = books[2].Id, Rating = 3, Content = "It was alright", UserId = users[1].Id},
            new Review { BookId = books[0].Id, Rating = 1, Content = "Really bad book! Don't recommend", UserId = users[2].Id},
            new Review { BookId = books[1].Id, Rating = 2, Content = "Meh.", UserId = users[2].Id},
            new Review { BookId = books[2].Id, Rating = 4, Content = "Good book!", UserId = users[2].Id}
        };

        foreach (var r in reviews)
        {
            context.Reviews.Add(r);
        }

        context.SaveChanges();
    }

    private static string  ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
}

