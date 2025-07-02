
using System;
using System.ComponentModel.DataAnnotations;

namespace PathbitChallenge.Domain.Entities
{
  public enum UserType
  {
    CLIENTE,
    ADMIN
  }

  public class User
  {
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public UserType UserType { get; set; }
  }
}
