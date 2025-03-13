namespace Unisphere.Explorer.Domain;

public class Booking
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public Guid HouseId { get; set; }

    public House House { get; set; }

    public DateOnly CheckIn { get; set; }

    public DateOnly CheckOut { get; set; }
}
