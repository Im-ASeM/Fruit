using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Identity;
using Kavenegar;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class LogginProcces : ILogginProcces
{    Context db ;
    private string papar = "Salam In Papar Man Hast MASALA";
    private KavenegarApi smsApi;
    public LogginProcces(){
        db = new Context();
        smsApi = new KavenegarApi(db.TokenSms.Find(1).token);
    }

    public List<SimpleUser> AllUser()
    {
        List<SimpleUser> results = new List<SimpleUser>();
        foreach(Users item in db.usersTbl.ToList()){
            SimpleUser result = new SimpleUser{ID = item.ID , UserName = item.UserName};
            results.Add(result);
        }
        return results;
    }

    public string DeleteUser(int userID)
    {
        Users check = db.usersTbl.Find(userID);
        if(check==null){
            return "User Not Found";
        }
        foreach (UserRoles item in db.UserRoleTbl.Where(x=> x.UserID==userID))
        {
            db.UserRoleTbl.Remove(item);
        }
        db.usersTbl.Remove(check);
        db.SaveChanges();
        return "Done";
        
        
    }

    public string Login(IdLessUser user)
    {
        Users check = db.usersTbl.FirstOrDefault(x=> x.UserName==user.UserName);
        if(check == null){
            return "Invalid User";
        }
        else if(!BCrypt.Net.BCrypt.Verify(user.Password+papar+user.UserName , check.Password )){
            return "Invalid Password";
        }
        else{
            return createToken(new User{ID = check.ID , UserName = user.UserName , Password = user.Password});
        }
    }

    public string Promote(int UserID , string Role)
    {
        Users check = db.usersTbl.Find(UserID);
        Roles checkRoll = db.RoleTbl.FirstOrDefault(x=> x.RoleName == Role);
        if(check == null){
            return "User Not Found";
        }
        else if(checkRoll == null){
            return "Invalid Roll";
        }
        // db.UserRoleTbl.Add(new UserRoles{UserID = UserID, UserRollID = checkRoll.ID}); 
        // برای چند رول ، من بلد نیستم چطور یه کاربر چندتا رول داشته باشه
        
        UserRoles result = db.UserRoleTbl.FirstOrDefault(x=> x.UserID == UserID);
        result.UserRollID = checkRoll.ID;
        db.UserRoleTbl.Update(result);
        db.SaveChanges();
        return $"{check.UserName} is {checkRoll.RoleName} Now ...";

    }

    public string Register(IdLessUser user)
    {   
        if(db.usersTbl.Any(x=> x.UserName==user.UserName)){
            return "USER IS ALLREADY ADDED";
        }

        Users result = new Users{
            UserName = user.UserName,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password+papar+user.UserName),
            CreateDate = DateTime.Now
        };

        db.usersTbl.Add(result);
        db.SaveChanges(); //check ...

        int userId = db.usersTbl.FirstOrDefault(x=> x.UserName==user.UserName).ID;

        UserRoles resultRole = new UserRoles{UserID = userId , UserRollID = db.RoleTbl.FirstOrDefault( x=> x.RoleName == "user").ID};
        db.UserRoleTbl.Add(resultRole);
        db.SaveChanges();

        return createToken(new User{ID = userId , UserName = user.UserName , Password = user.Password});

    }

    public string VerifyPassword(UpdateUser user)
    {
        Users check = db.usersTbl.FirstOrDefault(x=> x.UserName == user.UserName);
        if (check == null){
            return "Invalid UserName";
        }
        else if(check.ValidSms){
            if (user.SmsCode != check.SmsCode){
                check.ValidSms = false;
                db.usersTbl.Update(check);
                db.SaveChanges();
                return "Invalid Sms";
            }
            else{
                check.Password = BCrypt.Net.BCrypt.HashPassword(user.NewPassword+papar+user.UserName);
                check.ValidSms = false;
                check.CreateDate = DateTime.Now;
                db.usersTbl.Update(check);
                db.SaveChanges();
                return "Done !";
            }
        }
        else{
            return "Invalid Sms";
        }
    }

    public string RestorePassword(string UserName){
        Users check = db.usersTbl.FirstOrDefault(x=>UserName == x.UserName);
        if(check==null){
            return "Invalid User";
        }
        Random random = new Random();
        check.ValidSms = true;
        check.SmsCode = random.Next(100000,999999);
        smsApi.VerifyLookup(check.phone , check.SmsCode.ToString() , "demo");
        db.usersTbl.Update(check);
        db.SaveChanges();
        return "Code Sended";
    }

    private string createToken(User user){
        SymmetricSecurityKey privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sAlaM HalEt CheTory ? KhoBi ? Inja Chikar Mikoni Hacker Kalak ? Midonesti Kalak az har Taraf Kalake ?!?"));
        SigningCredentials credentials = new SigningCredentials(privateKey , SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer:"ASeM",
            audience:"ASeM",
            claims: new Claim[]{new Claim(ClaimTypes.Name , user.UserName),new Claim(ClaimTypes.Role , db.RoleTbl.Find(db.UserRoleTbl.FirstOrDefault(x=>x.UserID == user.ID).UserRollID).RoleName)},
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials : credentials
        );
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}