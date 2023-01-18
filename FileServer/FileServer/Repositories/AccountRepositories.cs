using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FileServer.Models.Account;
using FileServer.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FileServer.Repositories
{
    public class AccountRepositories : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _user_manager;
        private readonly SignInManager<IdentityUser> _user_sign;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountRepositories> _logger;

        public AccountRepositories(UserManager<IdentityUser> user_manager,
            SignInManager<IdentityUser> user_sign,
            IConfiguration configuration, ILogger<AccountRepositories> logger)
        {
            _user_manager = user_manager;
            _user_sign = user_sign;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IdentityResult> SingUpAccount(SignUpModel sign_up_model)
        {
            var user = new IdentityUser()
            {
                Email = sign_up_model.Email,
                UserName = sign_up_model.Username,
            };

            _logger.LogInformation($"Пользователь {sign_up_model.Username} успешно зарегестрировал аккаунт.");
            return await _user_manager.CreateAsync(user, sign_up_model.Password);
        }

        public async Task<AuthorizationModel> LoginAccount(SignInModel sign_in_model)
        {
            var result = await _user_sign.PasswordSignInAsync(sign_in_model.Username, sign_in_model.Password, false, false);

            if (!result.Succeeded) { return null; }

            var aut_claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Name, sign_in_model.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.CurrentCulture)),
                };
            var auth_sign_in_key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: aut_claims,
                signingCredentials: new SigningCredentials(auth_sign_in_key, SecurityAlgorithms.HmacSha256Signature)
                );

            AuthorizationModel authorization_model = new AuthorizationModel()
            {
                Username = sign_in_model.Username,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            _logger.LogInformation($"Пользователь {sign_in_model.Username} успешно зашёл в систему.");

            return authorization_model;
        }
    }
}
