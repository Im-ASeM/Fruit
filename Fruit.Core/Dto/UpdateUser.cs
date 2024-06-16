public class UpdateUser
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
    public string NewPassword { get; set; }
    public int SmsCode { get; set; }
}