using FluentValidation;
using GoogleCalendarIntegration.Application.Utils;
using GoogleCalendarIntegration.Domin.Models.GoogleCalendar;
using GoogleCalendarIntegration.Infrastructure.Reposatory;

namespace GoogleCalendarIntegration.Application.Validation.CalenderEventValidation
{
    internal class CreateCalenderEventValidation : AbstractValidator<CreateGoogleCalenderEventDto>
    {
        private readonly IGenericRepository<GoogleCalendarEvent> _googleCalendarEventRepo;
        private Language _language = Localizer.GetLanguage();

        public CreateCalenderEventValidation(IGenericRepository<GoogleCalendarEvent> googleCalendarEventRepo)
        {
            _googleCalendarEventRepo = googleCalendarEventRepo;

            RuleFor(u => u.Summary)
                .NotEmpty()
                .WithMessage(_language == Language.EN ? "Summary is Requred"
                : "عنوان الحدث مطلوب");

            RuleFor(u => u.Description)
               .NotEmpty()
               .WithMessage(_language == Language.EN ? "Description is Requred"
               : "تفاصيل الحدث مطلوبة");

            RuleFor(u => u.Email)
               .NotEmpty()
               .WithMessage(_language == Language.EN ? "Email is Requred"
               : "البريد الإلكتروني مطلوب");

            RuleFor(u => u.Start)
                .NotEmpty()
                .GreaterThan(DateTime.Now)
                .WithMessage(_language == Language.EN ? "the start date must be Greater than now"
                : "تاريخ البدء يجب ان يكون اكبر من التاريخ الحالي");

            RuleFor(u => u.End)
                .NotEmpty()
                .GreaterThan(u => u.Start)
                .WithMessage(_language == Language.EN ? "the end date must be Greater than start date"
                : "تاريخ البدء يجب ان يكون اكبر من تاريخ الإنتهاء");
        }
    }
}