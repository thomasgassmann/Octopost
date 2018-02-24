namespace Octopost.Model.Data
{
    using Microsoft.AspNetCore.Identity;

    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long? FacebookId { get; set; }

        public long? GoogleId { get; set; }
    }
}
