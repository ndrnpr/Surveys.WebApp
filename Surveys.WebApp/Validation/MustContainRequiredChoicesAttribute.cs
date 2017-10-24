using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Surveys.Models.ViewModels;
using Surveys.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Surveys.WebApp.Validation
{
    public class MustContainChoicesAttribute : ValidationAttribute
    {
        public MustContainChoicesAttribute() : base("Question {1} requires at least {0} options to be selected.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            BulletinViewModel bulletin = (BulletinViewModel)validationContext.ObjectInstance;

            foreach (var question in bulletin.SurveyInfo.Questions.Where(x => x.RequiredCount > 0))
            {
                var numberOfChoices = question.Choices.Where(x => bulletin.Votes.Contains(x.ChoiceID)).Count();

                if (numberOfChoices < question.RequiredCount)
                {
                    return new ValidationResult(FormatErrorMessage(question.QuestionNumber, question.RequiredCount, numberOfChoices));
                }
            }

            return ValidationResult.Success;
        }

        public string FormatErrorMessage(int questionNumber, int requiredCount, int selectedCount)
        {
            return string.Format(ErrorMessageString, requiredCount, questionNumber, selectedCount);
        }
    }
}
