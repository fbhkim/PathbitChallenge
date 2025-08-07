namespace PathbitChallenge.Domain.Entities;

public enum OrderStatus { PROCESSANDO, ENVIADO, CANCELADO }

public class Order
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTime OrderDate { get; set; } = DateTime.UtcNow;
  public OrderStatus Status { get; set; } = OrderStatus.PROCESSANDO;
  public Guid CustomerId { get; set; }
  public Guid ProductId { get; set; }
  public int Quantity { get; set; }
  public decimal TotalPrice { get; set; }
  public string DeliveryCep { get; set; } = string.Empty;
  public string DeliveryAddress { get; set; } = string.Empty;

 
  public Customer? Customer { get; set; }
  public Product? Product { get; set; }
}
