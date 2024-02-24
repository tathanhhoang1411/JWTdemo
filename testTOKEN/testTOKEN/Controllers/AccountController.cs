using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using testTOKEN.Model;

namespace testTOKEN.Controllers
{
    [Route("v1/api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpGet("login")]
        public IActionResult Get(string account, string pass)
        {
            try
            {
                IActionResult reponse = Unauthorized();
                if(CheckExistAccount(account, pass) == 1)
                {
                    Account acc=new Account();
                    acc.account=account;
                    acc.password = pass;
                    string Jwt = MakeToken(acc);
                    return Ok(new {token=Jwt});
                }
                return NotFound("Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest("Error");
            }
        }
        private int CheckExistAccount(string account, string pass)
        {
            try
            {
                Account acc = new Account();
                List<Account> lstAcc = acc.AccountList = acc.AddAccount();
                if (lstAcc.Count() > 0)
                {
                    var result = lstAcc.FirstOrDefault(acc => acc.account == account && acc.password == pass);
                if (result != null)
                    {
                        return 1;
                    }
                }
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        private string MakeToken(Account acc)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,acc.account),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
       return encodeToken;
        }
        [Authorize]//phương thức nào muốn xác thực token thì thên arrtribute này
        [HttpPost("Post")]
        public string Post()
        {
            var identity=HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Wellcome To: " + userName;
        }
        [Authorize]
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Value1", "Value2", "Value3" };
        }
    }
}
