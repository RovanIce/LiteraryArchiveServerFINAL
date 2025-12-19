using LiteraryArchiveServer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using pleaseworkplease;

namespace LiteraryArchiveServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(UserManager<Users> userManager, JwtHandling handler) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(loginRequest loginRequest)
        {
            Users? checkeduser = await userManager.FindByNameAsync(loginRequest.Username);
            if (checkeduser == null) { return Unauthorized("Invalid Username or password"); }
            bool paswordtesting = await userManager.CheckPasswordAsync(checkeduser,loginRequest.Password);
            if (!paswordtesting) { return Unauthorized("Invalid Username or password");}
            JwtSecurityToken token = await handler.GenerateToken(checkeduser);
            string string_token = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResponse
            {
                Sucess = true,
                Message = "loggin successful",
                Token = string_token
            });
        }
    }
}
