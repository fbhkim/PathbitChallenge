using PathbitChallenge.Application.DTOs;
using PathbitChallenge.Common;

namespace PathbitChallenge.Application.Interfaces;

public interface IAuthService
{
  Task<Result> SignUpAsync(SignUpRequest request);
  Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
}
