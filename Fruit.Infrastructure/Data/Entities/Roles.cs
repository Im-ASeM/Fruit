using System.ComponentModel.DataAnnotations;

public class Roles
{
    [Key]
    public int ID { get; set; }
    public string RoleName { get; set; }
    public DateTime CreateDate { get; set; }
}