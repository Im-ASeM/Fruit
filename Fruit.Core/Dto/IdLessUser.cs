public class IdLessUser
{
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