using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Data
{
    public class SeguridadDBContext : IdentityDbContext<Usuario>
    {
        public SeguridadDBContext(DbContextOptions<SeguridadDBContext> options ):base(options){}
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
