using System.ComponentModel.DataAnnotations;

public class NewUser
{
    private string _username;
    [Required]
    public string UserName {
        get{
            return _username;
        }
        set{
            _username=value.ToLower();
        }
    }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Phone { get; set; }
}