using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PathbitChallenge.Application.DTOs;
using PathbitChallenge.Application.Interfaces;
using PathbitChallenge.Common;
using PathbitChallenge.Domain.Entities;
using PathbitChallenge.Infrastructure.Data;

namespace PathbitChallenge.Application.Services;

public class OrderService : IOrderService
{
  private readonly AppDbContext _context;
  private readonly ICepService _cepService;
  private readonly ILogger<OrderService> _logger;

  public OrderService(AppDbContext context, ICepService cepService, ILogger<OrderService> logger)
  {
    _context = context;
    _cepService = cepService;
    _logger = logger;
  }

  public async Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request, Guid customerId)
  {
    // 1. Validar o endereço via CEP
    var addressInfo = await _cepService.GetAddressByCepAsync(request.DeliveryCep);
    if (addressInfo is null)
    {
      return Result.Failure<OrderResponse>("CEP inválido ou não encontrado.");
    }
    var fullAddress = $"{addressInfo.Endereco}, {addressInfo.Bairro}, {addressInfo.Cidade} - {addressInfo.Estado}";

    // 2. Usar uma transação para garantir a consistência
    await using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
      // 3. Validar se o produto existe e tem estoque
      var product = await _context.Products.FindAsync(request.ProductId);
      if (product is null)
      {
        return Result.Failure<OrderResponse>("Produto não encontrado.");
      }
      if (!product.HasStock(request.Quantity))
      {
        return Result.Failure<OrderResponse>($"Estoque insuficiente. Disponível: {product.AvailableQuantity}.");
      }

      // 4. Baixar o estoque
      product.DecreaseStock(request.Quantity);

      // 5. Criar o pedido (Order)
      var order = new Order
      {
        CustomerId = customerId,
        ProductId = request.ProductId,
        Quantity = request.Quantity,
        TotalPrice = product.Price * request.Quantity,
        DeliveryCep = request.DeliveryCep,
        DeliveryAddress = fullAddress,
        Status = OrderStatus.ENVIADO // Conforme requisito
      };

      _context.Orders.Add(order);
      await _context.SaveChangesAsync();
      await transaction.CommitAsync();

      _logger.LogInformation("Pedido {OrderId} criado com sucesso para o cliente {CustomerId}", order.Id, customerId);

      var response = new OrderResponse(order.Id, order.OrderDate, order.Status.ToString(), order.ProductId, product.Name, order.Quantity, order.TotalPrice, order.DeliveryAddress);
      return Result.Success(response);
    }
    catch (Exception ex)
    {
      await transaction.RollbackAsync();
      _logger.LogError(ex, "Erro ao criar pedido para o cliente {CustomerId}", customerId);
      return Result.Failure<OrderResponse>("Ocorreu um erro inesperado ao processar o pedido.");
    }
  }

  // Implemente os métodos de busca aqui...
  public Task<OrderResponse?> GetOrderByIdAsync(Guid id, Guid customerId)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<OrderResponse>> GetAllOrdersByCustomerAsync(Guid customerId)
  {
    throw new NotImplementedException();
  }
}
