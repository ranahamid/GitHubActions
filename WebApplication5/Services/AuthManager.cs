using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication5.Data;
using WebApplication5.Models;

namespace WebApplication5.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;
        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var token = new JwtSecurityToken();
            var life = jwtSettings.GetSection("Lifetime").Value;
            if (Int32.TryParse(life, out int expMin))
            {
                var expiration = DateTime.Now.AddMinutes(expMin);
                token = new JwtSecurityToken
                (
                    issuer: jwtSettings.GetSection("Issuer").Value,
                    audience: jwtSettings.GetSection("Audience").Value,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: signingCredentials);
            }
            return token;
        } 


        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName),
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim("fullName", _user.FirstName + " " + _user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"])
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private static SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("KEY");
            if (string.IsNullOrEmpty(key))
            {
                key = "755ec588-d5d9-4e86-aa9e-7e72d232968e";
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var cred = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            return cred;
        }

        public async Task<bool> ValidateUser(LoginDto userLoginDto)
        {
            _user = await _userManager.FindByNameAsync(userLoginDto.Email);
            var ispass = await _userManager.CheckPasswordAsync(_user, userLoginDto.Password);
            return _user != null && ispass;
        }

        public async Task<string> CreateRefreshToken()
        {
            try
            {
                await _userManager.RemoveAuthenticationTokenAsync(_user, "HotelListingApi", "RefreshToken");
                var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, "HotelListingApi", "RefreshToken");
                var result = await _userManager.SetAuthenticationTokenAsync(_user, "HotelListingApi", "RefreshToken", newRefreshToken);

                return newRefreshToken;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }

        public async Task<TokenRequest> VerifyRefreshToken(TokenRequest request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == ClaimTypes.Name)?.Value;
            _user = await _userManager.FindByNameAsync(username);
            try
            {
                var isValid = await _userManager.VerifyUserTokenAsync(_user, "HotelListingApi", "RefreshToken", request.RefreshToken);
                if (isValid)
                {
                    return new TokenRequest { Token = await CreateToken(), RefreshToken = await CreateRefreshToken() };
                }
                await _userManager.UpdateSecurityStampAsync(_user);
            }
            catch (Exception ex)
            {
                //throw ex;
            }

            return null;
        }
    }
}
