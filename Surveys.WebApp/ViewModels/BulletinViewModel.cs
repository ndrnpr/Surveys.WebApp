using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surveys.Models.ViewModels;
using Surveys.WebApp.Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Surveys.WebApp.ViewModels
{    
    public class BulletinViewModel : BulletinViewModelValidatable
    {
        public SurveyInfo SurveyInfo { get; set; }
        //[MustContainChoices]
        public IEnumerable<int> Votes { get; set; }

        public BulletinViewModel(IStringLocalizer stringLocalizer) : base(stringLocalizer)
        {            
        }
    }
}
