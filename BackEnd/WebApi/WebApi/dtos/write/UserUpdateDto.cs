using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOS;

public class UserUpdateDto
{
    /// <summary>
    /// contains the name of the User
    /// </summary>4
    [Required]
    public string name { get; set; }
}