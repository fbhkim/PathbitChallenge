using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathbitChallenge.Application.DTOs;
using PathbitChallenge.Domain.Entities;
using PathbitChallenge.Infrastructure.Data;

namespace PathbitChallenge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class ProductsController : ControllerBase
{
  private readonly AppDbContext _context;

  public ProductsController(AppDbContext context)
  {
    _context = context;
  }

  [HttpPost]
  [Authorize(Roles = "ADMINISTRADOR")]
  public async Task<IActionResult> CreateProduct(CreateProductRequest request)
  {
    var product = new Product
    {
      Name = request.Name,
      Price = request.Price,
      AvailableQuantity = request.AvailableQuantity
    };

    _context.Products.Add(product);
    await _context.SaveChangesAsync();

    var response = new ProductResponse(product.Id, product.Name, product.Price, product.AvailableQuantity);
    return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, response);
  }

  [HttpGet("{id}")]
  [AllowAnonymous] // Qualquer um pode ver um produto
  public async Task<IActionResult> GetProductById(Guid id)
  {
    var product = await _context.Products.FindAsync(id);
    if (product is null) return NotFound();

    var response = new ProductResponse(product.Id, product.Name, product.Price, product.AvailableQuantity);
    return Ok(response);
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> GetAllProducts()
  {
    var products = await _context.Products
        .Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.AvailableQuantity))
        .ToListAsync();

    return Ok(products);
  }

  [HttpPut("{id}")]
  [Authorize(Roles = "ADMINISTRADOR")]
  public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductRequest request)
  {
    var product = await _context.Products.FindAsync(id);
    if (product is null) return NotFound();

    product.Name = request.Name;
    product.Price = request.Price;
    product.AvailableQuantity = request.AvailableQuantity;

    await _context.SaveChangesAsync();
    return NoContent();
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "ADMINISTRADOR")]
  public async Task<IActionResult> DeleteProduct(Guid id)
  {
    var product = await _context.Products.FindAsync(id);
    if (product is null) return NotFound();

    _context.Products.Remove(product);
    await _context.SaveChangesAsync();
    return NoContent();
  }
}
