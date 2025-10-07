namespace IS.DTOs;

public class ReadOrderDTO
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public decimal TotalAmount { get; set; }

    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public ICollection<ReadOrderItemDTO> Items { get; set; } = new List<ReadOrderItemDTO>();
}
