namespace CardManager.Core.Entities
{
    public class CardRecord
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Track1 { get; set; }
        public string Track2 { get; set; }
        public string Track3 { get; set; }
        public DateOnly Created { get; set; }
        public Guid? CreatorId { get; set; }
        public User Creator { get; set; }
    }
}