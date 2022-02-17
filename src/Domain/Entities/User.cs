namespace Domain.Entities
{
    public class User : Audit
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
    }
}
