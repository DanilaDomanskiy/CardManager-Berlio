using CardManager.Api.Contracts.CardRecord;
using CardManager.Api.Contracts.CardRecords;
using CardManager.Api.Contracts.User;
using CardManager.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardManager.Api.Controllers
{
    [Authorize]
    [Route("api/card-records")]
    [ApiController]
    public class CardRecordsController : ControllerBase
    {
        private readonly ICardRecordsService _cardRecordsService;

        public CardRecordsController(ICardRecordsService cardRecordsService)
        {
            _cardRecordsService = cardRecordsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
        {
            var userIdClaim = User?.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
            if (Guid.TryParse(userIdClaim, out Guid currentUserId))
            {
                var result = await _cardRecordsService.AddRangeFromFileAsync(file, currentUserId, cancellationToken);
                return Ok(result.Select(r => new AddedCardRecordResponse
                {
                    Id = r.Id,
                    CardNumber = r.CardNumber,
                    Track1 = r.Track1,
                    Track2 = r.Track2,
                    Track3 = r.Track3,
                    Created = r.Created,
                    IsAdded = r.IsAdded
                }).ToList());
            }

            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 20,
            string? cardNumberStartsWith = null,
            string? creatorNameStartsWith = null,
            DateOnly? startCreationDate = null,
            DateOnly? endCreationDate = null)
        {
            var result = await _cardRecordsService.GetAsync(
                page,
                pageSize,
                cancellationToken,
                cardNumberStartsWith,
                creatorNameStartsWith,
                startCreationDate,
                endCreationDate);

            return Ok(new CardRecordsResponse
            {
                CardRecords = result.CardRecords.Select(r => new CardRecord
                {
                    Id = r.Id,
                    CardNumber = r.CardNumber,
                    Track1 = r.Track1,
                    Track2 = r.Track2,
                    Track3 = r.Track3,
                    Created = r.Created,
                    Creator = new UserResponse
                    {
                        Id = r.Creator.Id,
                        Name = r.Creator.Name,
                        Email = r.Creator.Email
                    }
                }),
                RecordsCount = result.RecordsCount,
            });
        }
    }
}