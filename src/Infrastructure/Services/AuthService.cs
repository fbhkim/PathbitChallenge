using Microsoft.EntityFrameworkCore;
using PathbitChallenge.Application.DTOs;
using PathbitChallenge.Application.Interfaces;
using PathbitChallenge.Common;
using PathbitChallenge.Domain.Entities;
using PathbitChallenge.Infrastructure.Data;

namespace PathbitChallenge.Application.Services;

public class AuthService : IAuthService
{
  private readonly AppDbContext _context;
  private readonly IPasswordHasher _passwordHasher;
  private readonly ITokenService _tokenService;

  public AuthService(AppDbContext context, IPasswordHasher passwordHasher, ITokenService tokenService)
  {
    _context = context;
    _passwordHasher = passwordHasher;
    _tokenService = tokenService;
  }

  public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
  {
    // Corrigido: buscar pelo Email, não Username
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
    {
      return Result.Failure<LoginResponse>("Usuário ou senha inválidos.");
    }

    var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == user.Email);
    if (customer is null)
    {
      return Result.Failure<LoginResponse>("Dados do cliente associado não encontrados.");
    }

    var token = _tokenService.GenerateToken(user, customer);
    return Result.Success(new LoginResponse(token));
  }

  public async Task<Result> SignUpAsync(SignUpRequest request)
  {
    // Corrigido: verificar se já existe um usuário com o mesmo Email
    var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
    if (userExists)
    {
      return Result.Failure("Um usuário com este e-mail já existe.");
    }

    var customer = new Customer
    {
      Name = request.Name,
      Email = request.Email,
    };

    var user = new User
    {
      Username = request.Email, // Usando o email como username
      Email = request.Email,
      PasswordHash = _passwordHasher.Hash(request.Password),
      UserType = UserType.CLIENTE // Por padrão, todo signup cria um CLIENTE
    };

    _context.Customers.Add(customer);
    _context.Users.Add(user);

    var success = await _context.SaveChangesAsync() > 0;
    return success ? Result.Success() : Result.Failure("Não foi possível salvar o usuário.");
  }
}
