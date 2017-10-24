using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Surveys.WebApp.SurveyServices.Base;
using Surveys.Models.ViewModels;
using Surveys.WebApp.ViewModels;
using Surveys.WebApp.Extensions;
using AutoMapper;
using Surveys.Models.Entities;
using Microsoft.Extensions.Localization;
using System.Reflection;
using OrchardCore.Localization.PortableObject;

namespace Surveys.WebApp.Controllers
{
    //[Route("[controller]/[action]")]
    public class SurveyController : Controller
    {
        private readonly IStringLocalizer _localizer;
        private readonly ISurveyService _surveyService;
        private readonly MapperConfiguration _config;

        public SurveyController(ISurveyService surveyService, IStringLocalizerFactory stringLocalizerFactory)
        {

            var type = typeof(SurveyController);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = stringLocalizerFactory.Create(type);

            //_localizer = stringLocalizer;
            _surveyService = surveyService;
            _config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BulletinViewModel, Bulletin>()
                    .ForMember(dest => dest.SurveyID, opt => opt.MapFrom(src => src.SurveyInfo.SurveyID))
                    .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes));
                cfg.CreateMap<int, Vote>()
                    .ForMember(dest => dest.ChoiceID, opt => opt.MapFrom(src => src));
            });
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = _localizer["Surveys"];
            ViewBag.Header = _localizer["Surveys"];
            var surveys = await _surveyService.GetSurveysAsync();
            if (surveys == null)
            {
                return NotFound();
            }
            return View(surveys);
        }

        [HttpGet("[controller]/[action]/{surveyid}")]
        public async Task<IActionResult> Details(int surveyid)
        {
            ViewBag.Title = _localizer["Survey Details"];
            ViewBag.Header = _localizer["Survey Details"];
            ViewBag.SurveyID = surveyid;
            object votes;
            if (!TempData.TryGetValue("votes", out votes))
            {
                votes = new HashSet<int>();
            }
            var survey = await _surveyService.GetSurveyAsync(surveyid);
            if (survey == null)
            {
                return NotFound();
            }
            var viewModel = new BulletinViewModel(_localizer)
            {
                SurveyInfo = survey,
                Votes = (IEnumerable<int>)votes
            };
            return View(viewModel);
        }

        [HttpGet("[controller]/[action]/{surveyid}")]
        public async Task<IActionResult> Results(int surveyid)
        {
            ViewBag.Title = _localizer["Survey Results"];
            ViewBag.Header = _localizer["Survey Results"];
            ViewBag.SurveyID = surveyid;
           
            var survey = await _surveyService.GetSurveyAsync(surveyid);
            if (survey == null)
            {
                return NotFound();
            }
            var viewModel = new BulletinViewModel(_localizer)
            {
                SurveyInfo = survey,
                Votes = new HashSet<int>()
            };
            return View(viewModel);
        }

        [HttpPost("[controller]/[action]/{surveyID}")]
        public async Task<IActionResult> Complete(int surveyID, IEnumerable<int> votes)
        {
            var survey = await _surveyService.GetSurveyAsync(surveyID);
            if (survey == null)
            {
                return NotFound();
            }
            var viewModel = new BulletinViewModel(_localizer)
            {
                SurveyInfo = survey,
                Votes = votes
            };

            if (!TryValidateModel(viewModel))
                return View(nameof(Details), viewModel);
            else
            {                
                var mapper = _config.CreateMapper();
                Bulletin bulletin = new Bulletin()
                {
                    SurveyID = surveyID,
                    Votes = mapper.Map<IEnumerable<Vote>>(votes)
                };
                var bulletin2 = mapper.Map<BulletinViewModel, Bulletin>(viewModel);
                await _surveyService.AddBulletinAsync(bulletin);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}