using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using DecodeWebApp.Config;
using DecodeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DecodeWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Login model, [FromServices]SigningConfigurations signingConfigurations, [FromServices]TokenConfigurations tokenConfigurations)
        {
            try
            {
                var login = ValidLogin(model);

                if (login)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new Claim[] 
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                            new Claim(ClaimTypes.Name, "Teste Nome"),
                            new Claim(ClaimTypes.Email, model.Username),
                            new Claim(ClaimTypes.Sid, "2")
                        }
                    );

                    DateTime dataCriacao = DateTime.Now;
                    DateTime dataExpiracao = dataCriacao +
                        TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = tokenConfigurations.Issuer,
                        Audience = tokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao,
                        IssuedAt = dataCriacao
                    });
                    var generateToken = handler.WriteToken(securityToken);

                    var token = new Token()
                    {
                        Authenticated = true,
                        Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        Value = generateToken,
                        Type = "Bearer"
                    };
                    return Ok(token);
                }
                else
                {
                    var token = new Token();
                    return Unauthorized(token);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool ValidLogin(Login login) => login.Username == "teste@teste.com" && login.Password == "123456";
    }
}