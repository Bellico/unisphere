namespace Unisphere.Explorer.Domain;

public class House
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    public Uri ImageUrl { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
    public ICollection<Booking> Bookings { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}
