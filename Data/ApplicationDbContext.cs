using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TechnicalConditions.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Appeal> Appeals { get; set; }
        public DbSet<AppealCase> AppealCases { get; set; }
        public DbSet<AppealerCategory> AppealerCategories { get; set; }
        public DbSet<AppealPurpose> AppealPurposes { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}
