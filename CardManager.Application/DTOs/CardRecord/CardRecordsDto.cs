namespace CardManager.Application.DTOs.CardRecord
{
    public class CardRecordsDto
    {
        public IEnumerable<CardRecordDto> CardRecords { get; set; }
        public int RecordsCount { get; set; }
    }
}