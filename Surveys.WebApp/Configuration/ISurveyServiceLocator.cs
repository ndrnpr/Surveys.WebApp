using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surveys.WebApp.Configuration
{
    public interface ISurveyServiceLocator
    {        
        string ServiceAddress { get; }        
    }
}
