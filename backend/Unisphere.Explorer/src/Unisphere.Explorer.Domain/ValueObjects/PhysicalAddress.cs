using Unisphere.Core.Common;

namespace Unisphere.Explorer.Domain;

public class PhysicalAddress : ValueObject
{
    public static PhysicalAddress Empty { get; } = new PhysicalAddress(string.Empty, string.Empty, string.Empty, string.Empty);

    public PhysicalAddress(string street, string zipCode, string city, string countryCode)
    {
        street = street.Trim();
        zipCode = zipCode.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);
        city = city.Trim();
        countryCode = countryCode.Trim();

        City = city;
        Street = street;
        ZipCode = zipCode;
        CountryCode = countryCode;
    }

    public string ZipCode { get; private set; }

    public string City { get; private set; }

    public string Street { get; private set; }

    public string CountryCode { get; private set; }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(City)
            && !string.IsNullOrEmpty(Street)
            && !string.IsNullOrEmpty(ZipCode)
            && !string.IsNullOrEmpty(CountryCode);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Street;
        yield return ZipCode;
        yield return CountryCode;
    }
}
