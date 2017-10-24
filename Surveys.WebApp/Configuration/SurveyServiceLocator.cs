using Microsoft.Extensions.Configuration;

namespace Surveys.WebApp.Configuration
{
    public class SurveyServiceLocator : ISurveyServiceLocator
    {
        public SurveyServiceLocator(IConfigurationRoot config)
        {
            var customSection = config.GetSection(nameof(SurveyServiceLocator));
            ServiceAddress = customSection?.GetSection(nameof(ServiceAddress))?.Value;
        }

        public string ServiceAddress { get; }
    }
}
