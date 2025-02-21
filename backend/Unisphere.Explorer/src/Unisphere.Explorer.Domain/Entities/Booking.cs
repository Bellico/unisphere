namespace Unisphere.Explorer.Domain;

public class Booking
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public Guid HouseId { get; set; }

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }
}
