/*
 https://fullstackmark.com/post/19/jwt-authentication-flow-with-refresh-tokens-in-aspnet-core-web-api
 */

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApplication6.Model;

namespace WebApplication6.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _config;
        private readonly ITokenFactory _tokenFactory = null;

        public AccountController(ILogger<AccountController> logger, IConfiguration config, ITokenFactory tokenFactory)
        {
            _logger = logger;
            _config = config;
            _tokenFactory = tokenFactory;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/generatetoken")]
        public IActionResult PostGenerateToken([FromForm] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == "test" && model.Password == "test")
                {

                    var claims = new[]
                    {
                      new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenProviderOptions:SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["TokenProviderOptions:Issuer"],
                      _config["TokenProviderOptions:Issuer"],
                      claims,
                      expires: DateTime.Now.AddMinutes(30),
                      signingCredentials: creds);

                    var refreshToken = _tokenFactory.GenerateToken();

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), refreshToken = refreshToken });
                }
            }

            return BadRequest("Could not create token");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/refreshtoken")]
        public IActionResult PostRefreshToken([FromForm] ExchangeRefreshTokenModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var claims = new[]
                    {
                      new Claim(JwtRegisteredClaimNames.Sub, "test"),
                      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenProviderOptions:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["TokenProviderOptions:Issuer"],
              _config["TokenProviderOptions:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            var refreshToken = _tokenFactory.GenerateToken();

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), refreshToken = refreshToken });
        }
    }
}