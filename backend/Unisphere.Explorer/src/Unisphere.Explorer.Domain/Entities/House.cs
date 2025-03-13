using System.Collections.ObjectModel;

namespace Unisphere.Explorer.Domain;

public class House
{
    public House()
    {
        Bookings = new Collection<Booking>();
    }

    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public string Name { get; set; }

    public PhysicalAddress PhysicalAddress { get; set; }

    public string Description { get; set; }

    public Notation Notation { get; set; }

    public Uri ImageUrl { get; set; }

    public ICollection<Booking> Bookings { get; }
}
