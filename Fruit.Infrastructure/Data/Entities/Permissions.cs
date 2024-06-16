using System.ComponentModel.DataAnnotations;

public class Permissions
{
    [Key]
    public int ID { get; set; }
    public int RollID { get; set; }
    public List<string> Access { get; set; }
    public bool Status { get; set; }
    public DateTime CreateDate { get; set; }
}