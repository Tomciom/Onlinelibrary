using System.ComponentModel.DataAnnotations;

namespace Onlinelibrary.Models;

public class ReviewViewModel
{
    
    public string Content { get; set; }

    public int Rating { get; set; }

    public int UserId { get; set; }
    
    public int BookId { get; set; }
    public string BookTitle { get; set; }
    public string AuthorName { get; set; }
}