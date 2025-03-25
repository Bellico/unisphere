using Microsoft.AspNetCore.Identity;

namespace Unisphere.Identity.Domain;

public class User : IdentityUser<Guid>
{
    public string ProfilPicture { get; set; }
}
