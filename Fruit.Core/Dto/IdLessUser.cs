using System.ComponentModel.DataAnnotations;

public class IdLessUser
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
}