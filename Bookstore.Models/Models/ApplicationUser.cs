using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
        public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }

        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
    }
}
