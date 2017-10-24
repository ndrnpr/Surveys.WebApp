using Microsoft.Extensions.Localization;
using Surveys.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Surveys.WebApp.Validation
{
    public class BulletinViewModelValidatable : IValidatableObject
    {
        private readonly IStringLocalizer _localizer;

        public BulletinViewModelValidatable(IStringLocalizer stringLocalizer)
        {
            _localizer = stringLocalizer;            
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            BulletinViewModel bulletin = (BulletinViewModel)validationContext.ObjectInstance;

            foreach (var question in bulletin.SurveyInfo.Questions.Where(x => x.RequiredCount > 0 || x.AllowedCount > 0))
            {
                var numberOfChoices = question.Choices.Where(x => bulletin.Votes.Contains(x.ChoiceID)).Count();

                if (numberOfChoices < question.RequiredCount)
                {
                    yield return new ValidationResult(
                        string.Format(_localizer["Question {1} requires at least {0} options to be selected."], question.QuestionNumber, question.RequiredCount)
                            + " "
                            + string.Format(_localizer["Your selection contains {0} options."], numberOfChoices));
                }
                if (numberOfChoices > question.AllowedCount)
                {
                    yield return new ValidationResult(
                        string.Format(_localizer["Question {1} allowed to select no more than {0} options."], question.QuestionNumber, question.RequiredCount)
                            + " "
                            + string.Format(_localizer["Your selection contains {0} options."], numberOfChoices));
                }
            }
        }
    }
}
