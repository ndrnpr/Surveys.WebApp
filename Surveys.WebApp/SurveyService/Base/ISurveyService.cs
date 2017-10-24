using Surveys.Models.Entities;
using Surveys.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surveys.WebApp.SurveyServices.Base
{
    public interface ISurveyService
    {
        Task<IEnumerable<SurveyStatsInfo>> GetSurveysAsync();
        Task<SurveyInfo> GetSurveyAsync(int id);

        Task<Bulletin> GetBulletinAsync(int id);

        Task<string> AddBulletinAsync(Bulletin bulletin);
    }
}
