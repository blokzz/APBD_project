namespace APBD_PROJEKT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Employee
{
    [Key]
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = null!;
}