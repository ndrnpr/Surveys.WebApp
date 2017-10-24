using Newtonsoft.Json;
using Surveys.WebApp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.WebApp.SurveyServices.Base
{
    public abstract class SurveyServiceBase
    {        
        protected readonly string ServiceAddress;
        protected readonly string SurveyBaseUri;
        protected readonly string BulletinBaseUri;

        protected SurveyServiceBase(ISurveyServiceLocator settings)
        {
            ServiceAddress = settings.ServiceAddress;
            SurveyBaseUri = $"{ServiceAddress}api/surveys/";
            BulletinBaseUri = $"{ServiceAddress}api/bulletins/";
        }

        internal async Task<string> GetJsonFromGetResponseAsync(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(uri);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"The Call to {uri} failed.  Status code: {response.StatusCode}");
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
        internal async Task<T> GetItemAsync<T>(string uri)
            where T : class, new()
        {
            try
            {
                var json = await GetJsonFromGetResponseAsync(uri);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        internal async Task<IList<T>> GetItemListAsync<T>(string uri)
            where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<IList<T>>(await GetJsonFromGetResponseAsync(uri));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        protected static async Task<string> ExecuteRequestAndProcessResponse(
            string uri, Task<HttpResponseMessage> task)
        {
            try
            {
                var response = await task;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"The Call to {uri} failed.  Status code: {response.StatusCode}");
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        protected StringContent CreateStringContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        protected async Task<string> SubmitPostRequestAsync(string uri, string json)
        {
            using (var client = new HttpClient())
            {
                var task = client.PostAsync(uri, CreateStringContent(json));
                return await ExecuteRequestAndProcessResponse(uri, task);
            }
        }

        protected async Task<string> SubmitObjectAsync<T>(string uri, T newobject)
        {
            var json = JsonConvert.SerializeObject(newobject);
            return await SubmitPostRequestAsync(uri, json);
        }

    }

}
