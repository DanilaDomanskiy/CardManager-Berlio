namespace CardManager.Api.Contracts.CardRecord
{
    public class CardRecordsResponse
    {
        public IEnumerable<CardRecord> CardRecords { get; set; }
        public int RecordsCount { get; set; }
    }
}