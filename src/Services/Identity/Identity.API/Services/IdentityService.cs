using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Identity.API.Configurations;
using Identity.API.Models;
using Identity.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace Identity.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;

        public IdentityService(JwtSettings jwtSettings, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }

        // need to hash user's password

        public async Task<AuthenticationResponse> Login(User userModel)
        {
            var user = await _userRepository.GetByEmailAsync(userModel.Email);

            if(user == null)
            {
                return new AuthenticationResponse { Success = false, ErrorMessages = new[] { "User not found" } };
            }

            if(user.Password != userModel.Password)
            {
                return new AuthenticationResponse { Success = false, ErrorMessages = new[] { "User/Password is incorrect" } };
            }

            return GenerateJwt(user);
        }

        public async Task<AuthenticationResponse> Register(User userModel)
        {
            if (await _userRepository.AnyByEmailAsync(userModel.Email))
            {
                return new AuthenticationResponse { Success = false, ErrorMessages = new[] { "Email already exists" } };
            }

            await _userRepository.AddAsync(userModel);
            await _userRepository.SaveChangesAsync();

            return GenerateJwt(userModel);
        }        

        private AuthenticationResponse GenerateJwt(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = GenerateTokenDescriptor(user);
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);

            return new AuthenticationResponse
            {
                Success = true,
                Token = jwt
            };
        }   

        private SecurityTokenDescriptor GenerateTokenDescriptor(User user)
        {
            var secretEncoded = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenClaims = GenerateClaimsPerUser(user);
            var tokenCredentials = new SigningCredentials(new SymmetricSecurityKey(secretEncoded), SecurityAlgorithms.HmacSha256);
            var tokenLifetime = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime);

            return new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = "payments",
                Subject = tokenClaims,
                Expires = tokenLifetime,
                SigningCredentials = tokenCredentials
            };
        }

        private ClaimsIdentity GenerateClaimsPerUser(User user)
        {
            return new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName + user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Email)
            });
        }

    }
}