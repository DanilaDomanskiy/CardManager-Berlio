namespace CardManager.Api.Contracts.CardRecords
{
    public class AddedCardRecordResponse
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