using FluentValidation;
using GoogleCalendarIntegration.Application.Utils;
using GoogleCalendarIntegration.Domin.Models.GoogleCalendar;
using GoogleCalendarIntegration.Infrastructure.Reposatory;

namespace GoogleCalendarIntegration.Application.Validation.CalenderEventValidation
{
    internal class UpdateCalenderEventValidation : AbstractValidator<UpdateGoogleCalenderEventDto>
    {
        private readonly IGenericRepository<GoogleCalendarEvent> _googleCalendarEventRepo;
        private Language _language = Localizer.GetLanguage();

        public UpdateCalenderEventValidation(IGenericRepository<GoogleCalendarEvent> googleCalendarEventRepo)
        {
            _googleCalendarEventRepo = googleCalendarEventRepo;

            RuleFor(u => u.Id)
                .NotEmpty()
                .MustAsync(IsExist)
                .WithMessage(_language == Language.EN ? "Event not Found"
                : "الحدث غير موجود");

            RuleFor(u => u.Start)
                .GreaterThan(DateTime.Now)
                .WithMessage(_language == Language.EN ? "the start date must be Greater than now"
                : "تاريخ البدء يجب ان يكون اكبر من التاريخ الحالي");

            RuleFor(u => u.End)
                .GreaterThan(u => u.Start)
                .WithMessage(_language == Language.EN ? "the end date must be Greater than start date"
                : "تاريخ البدء يجب ان يكون اكبر من تاريخ الإنتهاء");
        }

        private async Task<bool> IsExist(long id, CancellationToken cancellationToken)
            => (await _googleCalendarEventRepo.IsExist(x => x.Id == id));
    }
}