using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppDist.Scaffolds;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Configuration;
using Microsoft.Extensions.Options;


namespace AppDist.Controllers
{
    [ApiController]
    [Route("Authenticate")]
    public class AuthenticationController : ControllerBase
    {
        private readonly schoolapiContext context;
        private readonly AppSettings _appsettings;

        public AuthenticationController(schoolapiContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this._appsettings = appSettings.Value;

        }

                [HttpPost]
        public async Task<IActionResult> authenticate([FromBody] Admin admin)
        {
            // Console.WriteLine(admin.Email);
            // Console.WriteLine(admin.Password);
            // var _admin = await context.Admin.Where(x => x.Email == admin.Email).FirstAsync();
            // if (admin == null)
            // {
            //     return NotFound("Username" + admin.Email + "was not found on the server");
            // }
            // SHA1 sha = new SHA1CryptoServiceProvider();
            // byte[] bytes = ASCIIEncoding.UTF8.GetBytes(admin.Password);
            // byte[] computedhash = sha.ComputeHash(bytes);
            // String _password = BitConverter.ToString(computedhash).Replace("-", "").ToLower();
            // if (_password == _admin.Password)
            //{
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, admin.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok("{\"token\":\"" + tokenHandler.WriteToken(token) + "\"}");
            //}
            //   return NotFound();
        }
    }
}
