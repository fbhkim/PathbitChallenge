using PathbitChallenge.Domain.Entities;

namespace PathbitChallenge.Application.Interfaces;

public interface ITokenService
{
  string GenerateToken(User user, Customer customer);
}
