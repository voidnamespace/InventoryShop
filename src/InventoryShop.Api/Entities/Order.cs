using IS.Entities;

public class Order
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public decimal TotalAmount { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
