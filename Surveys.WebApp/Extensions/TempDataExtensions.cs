using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surveys.WebApp.Extensions
{
    public static class TempDataExtensions
    {
        //public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
        //{
        //    tempData.Set<string>(key, JsonConvert.SerializeObject(value));
        //}

        public static T Get<T>(this ITempDataDictionary tempData, string key)
        {
            object value;
            if (tempData.TryGetValue(key, out value))
            {
                return JsonConvert.DeserializeObject<T>((string)value);
            }
            else
            {
                return default(T);
            }
        }
    }
}
