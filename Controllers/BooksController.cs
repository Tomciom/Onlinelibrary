using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Onlinelibrary.Models;
using Onlinelibrary.Data;
using System.Security.Claims;

namespace Onlinelibrary.Controllers;

public class BooksController : Controller
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var books = _context.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .ToList();
        return View(books);
    }

    public IActionResult Details(int id)
    {
        var book = _context.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefault(b => b.Id == id);
        return View(book);
    }

    public IActionResult Create()
    {
        return View(new CreateViewModel());
    }
    
    [HttpPost]
   public IActionResult Create(CreateViewModel model)
    {
        if (ModelState.IsValid)
            {  
                var book = new Book
                {
                    Title = model.Title,
                    Author = new Author 
                    {
                        Name = model.Author,
                    },
                    Genre = model.Genre,
                };
                    
                _context.Books.Add(book);
                _context.SaveChanges();
                return RedirectToAction("Index", "Books");
            }
        return View(model);  
    }

    
    public IActionResult AddReview(int id)
    {
        
        var userid = HttpContext.Session.GetString("UserId");

        Console.WriteLine("User ID: " + userid);

        var book = _context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        var model = new ReviewViewModel 
        { 
            BookId = id ,
            BookTitle = book.Title,
            AuthorName = book.Author.Name,
            UserId = int.Parse(userid)
        };
        return View(model);
    }

    
    [HttpPost]
    public IActionResult AddReview(ReviewViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var userid = HttpContext.Session.GetString("UserId");
            
            var review = new Review
            {
                Content = model.Content,
                Rating = model.Rating,
                BookId = model.BookId,
                UserId = int.Parse(userid)

                
            };
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return RedirectToAction("Index", "Books");
        }

        var book = _context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == model.BookId);
        if (book != null)
        {
            model.BookTitle = book.Title;
            model.AuthorName = book.Author.Name;
        }
        return View(model);
    }

    
    public IActionResult CreateAuthor()
    {
        return View();
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAuthor(Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create));
        }
        return View(author);
    }

    [HttpGet]
    public IActionResult TopRatedBooks()
    {
        var books = _context.Books
                            .Include(b => b.Reviews)
                            .Include(b => b.Author)
                            .OrderByDescending(b => b.Reviews.Average(r => r.Rating))
                            .Take(10)
                            .ToList();
        return View(books);
    }
}
