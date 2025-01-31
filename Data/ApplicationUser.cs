using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechnicalConditions.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
		[DataType(DataType.Date)]
		public DateTime? Birtsday { get; set; }
		public string? FirstName { get; set; }
		public char? Gender { get; set; }
	}
}
