using Microsoft.EntityFrameworkCore;

namespace MembersClub.Model
{
    public class MembersClubDbContext : DbContext
    {
        public MembersClubDbContext(DbContextOptions<MembersClubDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Member { get; set; }
    }
}
