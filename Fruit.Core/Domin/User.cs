public class User
{
    public int ID { get; set; }
    private string _username;
    public string UserName {
        get{
            return _username;
        }
        set{
            _username=value.ToLower();
        }
    }
    public string Password { get; set; }
}