using CardManager.Application.Validators;
using CardManager.Core.Entities;
using CardManager.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CardManager.Infrastructure.Repositories
{
    public class CardRecordsRepository : ICardRecordsRepository
    {
        private readonly WebContext _webContext;

        public CardRecordsRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public async Task<IEnumerable<(CardRecord Record, bool IsAdded)>> CreateRangeAsync(IEnumerable<CardRecord> cardRecords, CancellationToken cancellationToken)
        {
            var validator = new CardRecordValidator();

            var cardNumbers = cardRecords
                .Select(c => c.CardNumber)
                .ToList();

            var existingCardNumbers = await _webContext.CardRecords
                .Where(c => cardNumbers.Contains(c.CardNumber))
                .Select(c => c.CardNumber)
                .ToListAsync(cancellationToken);

            var result = new List<(CardRecord, bool)>();
            var newCardRecords = new List<CardRecord>();

            foreach (var record in cardRecords)
            {
                if (existingCardNumbers.Contains(record.CardNumber) || !validator.Validate(record).IsValid)
                {
                    result.Add((record, false));
                }
                else
                {
                    newCardRecords.Add(record);
                    result.Add((record, true));
                }
            }

            if (newCardRecords.Any())
            {
                await _webContext.CardRecords.AddRangeAsync(newCardRecords, cancellationToken);
                await _webContext.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        public async Task<(IEnumerable<CardRecord> CardRecords, int RecordsCount)> ReadCardRecordsAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken,
            string? cardNumberStartsWith = null,
            string? creatorNameStartsWith = null,
            DateOnly? startCreationDate = null,
            DateOnly? endCreationDate = null)
        {
            var query = _webContext.CardRecords.Include(cr => cr.Creator).AsNoTracking();

            if (!string.IsNullOrEmpty(cardNumberStartsWith))
                query = query.Where(r => r.CardNumber.StartsWith(cardNumberStartsWith));

            if (!string.IsNullOrEmpty(creatorNameStartsWith))
                query = query.Where(r => r.Creator.Name.StartsWith(creatorNameStartsWith));

            if (startCreationDate.HasValue)
                query = query.Where(r => r.Created >= startCreationDate);

            if (endCreationDate.HasValue)
                query = query.Where(r => r.Created <= endCreationDate);

            var totalRecordsCount = await query.CountAsync(cancellationToken);

            var records = await query
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (records, totalRecordsCount);
        }
    }
}