using PathbitChallenge.Application.DTOs;
using PathbitChallenge.Common;

namespace PathbitChallenge.Application.Interfaces;

public interface IOrderService
{
  Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request, Guid customerId);
  Task<OrderResponse?> GetOrderByIdAsync(Guid id, Guid customerId);
  Task<IEnumerable<OrderResponse>> GetAllOrdersByCustomerAsync(Guid customerId);
}
