using System.ComponentModel.DataAnnotations;

public class Users
{
    [Key]
    public int ID { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public String Password { get; set; }
}