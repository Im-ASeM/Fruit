using System.ComponentModel.DataAnnotations;

public class Users
{
    [Key]
    public int ID { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public String Password { get; set; }
    public bool ValidSms { get; set; }
    public string phone { get; set; }
    public int SmsCode { get; set; }
    public DateTime CreateDate { get; set; }
    public Users()
    {
        UserName = "Invalid";
        Password = "password";
        ValidSms = false;
        phone = "phone?";
        SmsCode = -1;
        CreateDate = DateTime.Now;
    }
}