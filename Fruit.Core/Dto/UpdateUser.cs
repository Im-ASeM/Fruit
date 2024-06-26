using System.ComponentModel.DataAnnotations;

public class UpdateUser
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
    public string NewPassword { get; set; }
    [Required]
    public int SmsCode { get; set; }
}