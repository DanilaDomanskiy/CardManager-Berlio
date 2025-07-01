using CardManager.Application.DTOs.CardRecord;
using Microsoft.AspNetCore.Http;

namespace CardManager.Application.Services.Interfaces
{
    public interface ICardRecordsService
    {
        Task<IEnumerable<AddedCardRecordDto>> AddRangeFromFileAsync(IFormFile file, Guid currentUserId, CancellationToken cancellationToken);

        Task<CardRecordsDto> GetAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken,
            string? cardNumberStartsWith = null,
            string? creatorNameStartsWith = null,
            DateOnly? startCreationDate = null,
            DateOnly? endCreationDate = null);
    }
}