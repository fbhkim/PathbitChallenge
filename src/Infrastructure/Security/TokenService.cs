using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PathbitChallenge.Application.Interfaces;
using PathbitChallenge.Domain.Entities;

namespace PathbitChallenge.Infrastructure.Security;

public class TokenService : ITokenService
{
  private readonly IConfiguration _configuration;

  public TokenService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string GenerateToken(User user, Customer customer)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("customerId", customer.Id.ToString()), 
                new Claim(ClaimTypes.Name, customer.Name),
                new Claim(ClaimTypes.Role, user.UserType.ToString()) 
      Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["Jwt:DurationInHours"])),
      Issuer = _configuration["Jwt:Issuer"],
      Audience = _configuration["Jwt:Audience"],
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}
