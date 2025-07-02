namespace PathbitChallenge.Domain.Entities;

public class Product
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public int AvailableQuantity { get; set; }

  public bool HasStock(int quantity) => AvailableQuantity >= quantity;

  public void DecreaseStock(int quantity)
  {
    if (!HasStock(quantity))
    {
      throw new InvalidOperationException("Estoque insuficiente para este produto.");
    }
    AvailableQuantity -= quantity;
  }
}
