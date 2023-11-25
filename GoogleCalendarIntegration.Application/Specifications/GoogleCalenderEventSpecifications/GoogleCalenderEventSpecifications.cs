using GoogleCalendarIntegration.Domin.Models.GoogleCalendar;
using GoogleCalendarIntegration.Domin.Specifications;

namespace GoogleCalendarIntegration.Application.Specifications
{
    internal class GoogleCalenderEventSpecifications : BaseSpecification<GoogleCalendarEvent>
    {
        public GoogleCalenderEventSpecifications(GoogleCalenderEventRequstModel requestModel)
        {
            if (!String.IsNullOrEmpty(requestModel.Search))
                AddCriteria(c => c.Summary.ToLower().Contains(requestModel.Search.ToLower())
                || c.Description.ToLower().Contains(requestModel.Search.ToLower()));

            if (requestModel.From != null)
                AddCriteria(x => x.Start > requestModel.From);

            if (requestModel.To != null)
                AddCriteria(x => x.End < requestModel.To);

            if(!String.IsNullOrEmpty(requestModel.Email))
                AddCriteria(c => c.CalendarIdInGoogle == requestModel.Email);

            ApplyPaging(requestModel.PageSize, requestModel.PageIndex);
        }

        public GoogleCalenderEventSpecifications(long Id)
        {
            AddCriteria(c => c.Id == Id);
        }
    }
}
