using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;
using CardManager.Core.Entities;
using Microsoft.AspNetCore.Http;
using CardManager.Application.Services.Interfaces;
using CardManager.Core.IRepositories;
using CardManager.Application.DTOs.CardRecord;
using CardManager.Application.DTOs.User;

namespace CardManager.Application.Services
{
    public class CardRecordsService : ICardRecordsService
    {
        private readonly ICardRecordsRepository _cardRecordRepository;

        public CardRecordsService(ICardRecordsRepository cardRecordRepository)
        {
            _cardRecordRepository = cardRecordRepository;
        }

        public async Task<IEnumerable<AddedCardRecordDto>> AddRangeFromFileAsync(IFormFile file, Guid currentUserId, CancellationToken cancellationToken)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false,
                Encoding = Encoding.UTF8,
                IgnoreBlankLines = true
            };

            using var fileReader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
            using var csvReader = new CsvReader(fileReader, config);

            var records = new List<CardRecord>();

            if (!DateOnly.TryParseExact(
                file.FileName[..6], "yyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly creationTime))
            {
                creationTime = DateOnly.FromDateTime(DateTime.Today);
            }

            while (await csvReader.ReadAsync())
            {
                cancellationToken.ThrowIfCancellationRequested();

                var record = csvReader.Parser.Record;

                records.Add(new CardRecord
                {
                    CardNumber = record[0],
                    Track1 = record[1],
                    Track2 = record[2],
                    Track3 = record[3],
                    Created = creationTime,
                    CreatorId = currentUserId
                });
            }

            var result = await _cardRecordRepository.CreateRangeAsync(records, cancellationToken);

            return result.Select(r => new AddedCardRecordDto
            {
                Id = r.Record.Id,
                CardNumber = r.Record.CardNumber,
                Track1 = r.Record.Track1,
                Track2 = r.Record.Track2,
                Track3 = r.Record.Track3,
                Created = r.Record.Created,
                IsAdded = r.IsAdded
            }).ToList();
        }

        public async Task<CardRecordsDto> GetAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken,
            string? cardNumberStartsWith = null,
            string? creatorNameStartsWith = null,
            DateOnly? startCreationDate = null,
            DateOnly? endCreationDate = null)
        {
            var records = await _cardRecordRepository.ReadCardRecordsAsync(
                page,
                pageSize,
                cancellationToken,
                cardNumberStartsWith,
                creatorNameStartsWith,
                startCreationDate,
                endCreationDate);

            return new CardRecordsDto
            {
                CardRecords = records.CardRecords.Select(r => new CardRecordDto
                {
                    Id = r.Id,
                    CardNumber = r.CardNumber,
                    Track1 = r.Track1,
                    Track2 = r.Track2,
                    Track3 = r.Track3,
                    Created = r.Created,
                    Creator = new UserDto
                    {
                        Id = r.Creator.Id,
                        Name = r.Creator.Name,
                        Email = r.Creator.Email
                    }
                }),
                RecordsCount = records.RecordsCount,
            };
        }
    }
}