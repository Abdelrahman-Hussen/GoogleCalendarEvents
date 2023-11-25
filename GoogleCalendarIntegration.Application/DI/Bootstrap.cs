using FluentValidation;
using GoogleCalendarIntegration.Application.Abstractions;
using GoogleCalendarIntegration.Application.Services;
using GoogleCalendarIntegration.Application.Validation.CalenderEventValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GoogleCalendarIntegration.Application.DI
{
    public static class Bootstrap
    {
        public static IServiceCollection ApplicationStrapping(this IServiceCollection services)
        {

            #region Services

            services.AddScoped<IGoogleAuthService, GoogleAuthService>();

            services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();

            #endregion


            #region Validators

            services.AddScoped<IValidator<CreateGoogleCalenderEventDto>, CreateCalenderEventValidation>();
            services.AddScoped<IValidator<UpdateGoogleCalenderEventDto>, UpdateCalenderEventValidation>();

            #endregion


            #region Mappster

            var config = TypeAdapterConfig.GlobalSettings;

            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);

            TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
            
            services.AddScoped<IMapper, ServiceMapper>();

            #endregion


            return services;
        }
    }
}
