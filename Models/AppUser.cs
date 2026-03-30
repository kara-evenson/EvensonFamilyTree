namespace EvensonFamilyTreeAppsDev.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public int? PersonId { get; set; }
        public Person? Person { get; set; }

        public int? UserTypeId { get; set; }
        public UserType? UserType { get; set; }
    }
}
