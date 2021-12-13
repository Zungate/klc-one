using Microsoft.AspNetCore.Identity;

namespace klc_one.Models;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}
