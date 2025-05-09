namespace Diploma.Models
{
    public class Workers
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public bool IsAdmin { get; set; }
    }
}
