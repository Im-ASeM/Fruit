using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[Action]")]
[ApiController]
public class Controllers:Controller
{
    ILogginProcces lp;
    public Controllers(ILogginProcces _lp)
    {
        lp=_lp;
    }

    [HttpPost]
    public IActionResult Login(IdLessUser user){
        return Ok(lp.Login(user));
    }

    [HttpPost]
    public IActionResult Register(IdLessUser user){
        return Ok(lp.Register(user));
    }
    [HttpGet]
    [Authorize]
    public IActionResult SelectFruits(){
        List<string> fruits = new List<string>{"sib","kivi","moz","ANNANNAS!","holo","gilas"};
        return Ok(fruits); 
    }
}