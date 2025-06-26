using CardManager.Core.Entities;

namespace CardManager.Core.IRepositories
{
    public interface ICardRecordsRepository
    {
        Task<IEnumerable<(CardRecord Record, bool IsAdded)>> CreateRangeAsync(IEnumerable<CardRecord> cardRecords, CancellationToken cancellationToken);

        Task<(IEnumerable<CardRecord> CardRecords, int RecordsCount)> ReadCardRecordsAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken,
            string? cardNumberStartsWith = null,
            string? creatorNameStartsWith = null,
            DateOnly? startCreationDate = null,
            DateOnly? endCreationDate = null);
    }
}