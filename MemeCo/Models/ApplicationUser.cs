using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
    public string Bio { get; set; }
    [Required]
    public bool DarkMode { get; set; }

    public IEnumerable<ApplicationUser> Followers { get; set; }

    public IEnumerable<ApplicationUser> Following { get; set; }
    //TODO: fix
    public IEnumerable<ApplicationUser> Posts { get; set; }
    public IEnumerable<ApplicationUser> Likes { get; set; }
    public IEnumerable<ApplicationUser> Comments { get; set; }

}