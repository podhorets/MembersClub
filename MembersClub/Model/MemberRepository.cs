namespace MembersClub.Model
{
    public class MemberRepository : Repository<Member, MembersClubDbContext>
    {
        public MemberRepository(MembersClubDbContext context) : base(context)
        {

        }
    }
}
