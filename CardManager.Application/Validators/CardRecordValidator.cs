using CardManager.Core.Entities;
using FluentValidation;

namespace CardManager.Application.Validators
{
    public class CardRecordValidator : AbstractValidator<CardRecord>
    {
        public CardRecordValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .MaximumLength(20)
                .Matches("^[0-9]+$");

            RuleFor(x => x.Track1)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.Track2)
                .NotEmpty()
                .MaximumLength(40);

            RuleFor(x => x.Track3)
                .NotEmpty()
                .MaximumLength(110);
        }
    }
}