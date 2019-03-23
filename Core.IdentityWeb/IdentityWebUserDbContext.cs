using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.IdentityWeb
{
    public class IdentityWebUserDbContext: IdentityDbContext<IdentityWebUser>
    {
        public IdentityWebUserDbContext(DbContextOptions<IdentityWebUserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityWebUser>(user => user.HasIndex(x => x.Locale).IsUnique(false));
            builder.Entity<Organization>(org => {
                org.ToTable("Organizations");
                org.HasKey(x => x.Id);
                org.HasMany<IdentityWebUser>().WithOne().HasForeignKey(x => x.OrgId).IsRequired(false);
            });
        }
    }
}
