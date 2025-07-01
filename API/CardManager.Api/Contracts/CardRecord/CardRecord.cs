using CardManager.Api.Contracts.User;

namespace CardManager.Api.Contracts.CardRecord
{
    public class CardRecord
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Track1 { get; set; }
        public string Track2 { get; set; }
        public string Track3 { get; set; }
        public DateOnly Created { get; set; }
        public UserResponse Creator { get; set; }
    }
}