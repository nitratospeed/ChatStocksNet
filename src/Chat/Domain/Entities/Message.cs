namespace Domain.Entities
{
    public class Message : Audit
    {
        public string User { get; set; }
        public string Text { get; set; }
        public string Room { get; set; }
    }
}
