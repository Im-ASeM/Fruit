using System.ComponentModel.DataAnnotations;

public class smsToken
{
    [Key]
    public int ID { get; set; }
    public string token { get; set; }
}