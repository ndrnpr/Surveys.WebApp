using Surveys.Models.Entities;
using Surveys.Models.ViewModels;
using Surveys.WebApp.Configuration;
using Surveys.WebApp.SurveyServices.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surveys.WebApp.SurveyServices
{
    public class SurveyService : SurveyServiceBase, ISurveyService
    {
        public SurveyService(ISurveyServiceLocator settings) : base(settings)
        {
        }

        public async Task<IEnumerable<SurveyStatsInfo>> GetSurveysAsync()
        {
            //http://localhost:40001/api/surveys
            return await GetItemListAsync<SurveyStatsInfo>(SurveyBaseUri);
        }
        public async Task<SurveyInfo> GetSurveyAsync(int id)
        {
            //http://localhost:40001/api/surveys/{id}
            return await GetItemAsync<SurveyInfo>($"{SurveyBaseUri}{id}");
        }

        public async Task<Bulletin> GetBulletinAsync(int id)
        {
            return await GetItemAsync<Bulletin>($"{BulletinBaseUri}{id}");
        }

        public async Task<string> AddBulletinAsync(Bulletin bulletin)
        {
            string uri = $"{BulletinBaseUri}";
            return await SubmitObjectAsync(uri, bulletin);
        }

    }
}
