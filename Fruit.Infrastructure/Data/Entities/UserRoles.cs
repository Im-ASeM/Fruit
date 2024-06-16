using System.ComponentModel.DataAnnotations;

public class UserRoles
{
    [Key]
    public int ID { get; set; }
    public int UserID { get; set; }
    public int UserRollID { get; set; }
}