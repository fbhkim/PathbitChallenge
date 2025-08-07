/// Exemplo de como estruturar um teste
using Moq;
using FluentAssertions;
using PathbitChallenge.Application.Interfaces;
using PathbitChallenge.Application.Services;
using PathbitChallenge.Domain.Entities;
using PathbitChallenge.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using PathbitChallenge.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace PathbitChallenge.UnitTests;

public class AuthServiceTests
{
  private readonly Mock<IPasswordHasher> _passwordHasherMock;
  private readonly Mock<ITokenService> _tokenServiceMock;
  private readonly AppDbContext _context;

  public AuthServiceTests()
  {
    _passwordHasherMock = new Mock<IPasswordHasher>();
    _tokenServiceMock = new Mock<ITokenService>();

    
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
    _context = new AppDbContext(options);
  }

  [Fact]
  public async Task SignUpAsync_ShouldReturnSuccess_WhenUserDoesNotExist()
  {
    
    var request = new SignUpRequest("Test User", "test@test.com", "password123");
    _passwordHasherMock.Setup(p => p.Hash(It.IsAny<string>())).Returns("hashed_password");

    var authService = new AuthService(_context, _passwordHasherMock.Object, _tokenServiceMock.Object);

   
    var result = await authService.SignUpAsync(request);

    
    result.IsSuccess.Should().BeTrue();
    _context.Users.Should().HaveCount(1);
    _context.Customers.Should().HaveCount(1);
  }
}
