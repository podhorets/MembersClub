namespace MembersClub.Model
{
    public class Member : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RegistrationDate { get; set; } = DateTime.Now.ToString("dd.MM.yyyy");
    }
}