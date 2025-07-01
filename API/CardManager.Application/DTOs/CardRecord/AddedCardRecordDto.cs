namespace CardManager.Application.DTOs.CardRecord
{
    public class AddedCardRecordDto
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Track1 { get; set; }
        public string Track2 { get; set; }
        public string Track3 { get; set; }
        public DateOnly Created { get; set; }
        public bool IsAdded { get; set; }
    }
}