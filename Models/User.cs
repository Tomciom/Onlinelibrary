namespace Onlinelibrary.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public bool isAdmin { get; set; }
    public List<Review> Reviews { get; set; } = new List<Review>();

}