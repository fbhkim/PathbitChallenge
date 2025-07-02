using System.Security.Cryptography;
using System.Text;
using PathbitChallenge.Application.Interfaces;

namespace PathbitChallenge.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
  public string Hash(string password)
  {
    using var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(password);
    var hashBytes = sha256.ComputeHash(bytes);
    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
  }

  public bool Verify(string password, string hashedPassword)
  {
    var hashOfInput = Hash(password);
    return StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, hashedPassword) == 0;
  }
}
