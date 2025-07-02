using System.ComponentModel.DataAnnotations;

namespace PathbitChallenge.Application.DTOs;

// Auth
public record SignUpRequest(
    [Required] string Name,
    [Required][EmailAddress] string Email,
    [Required] string Password);

public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password);

public record LoginResponse(string Token);

// Product
public record CreateProductRequest(
    [Required] string Name,
    [Range(0.01, double.MaxValue)] decimal Price,
    [Range(1, int.MaxValue)] int AvailableQuantity);

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int AvailableQuantity);

public record UpdateProductRequest(
    [Required] string Name,
    [Range(0.01, double.MaxValue)] decimal Price,
    [Range(0, int.MaxValue)] int AvailableQuantity);

// Order
public record CreateOrderRequest(
    [Required] Guid ProductId,
    [Range(1, int.MaxValue)] int Quantity,
    [Required][StringLength(8)] string DeliveryCep);

public record OrderResponse(
    Guid Id,
    DateTime OrderDate,
    string Status,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal TotalPrice,
    string DeliveryAddress);

// Customer
public record CustomerResponse(
    Guid Id,
    string Name,
    string Email);
