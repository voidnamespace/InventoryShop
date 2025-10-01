namespace IS.DTOs;

public class CreateOrderDTO
{
    public List<CreateOrderItemDTO> Items { get; set; } = new List<CreateOrderItemDTO>();
}
