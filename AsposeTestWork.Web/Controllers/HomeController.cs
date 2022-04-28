using AsposeTestWork.Web.Models;
using AsposeTestWork.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using AsposeTestWork.Web.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;

namespace AsposeTestWork.Web.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private HomeViewModel _model;
        HttpClient _httpClient = new HttpClient();

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _logger = logger;
            _environment = hostEnvironment;
            _configuration = configuration;
            _model = new HomeViewModel(hostEnvironment, configuration);
        }

        public IActionResult Index()
        {
            var translater = new Core.Translaters.Yandex.Translater(_configuration);
            var Cultures = translater.GetSupportedLanguages();
            ViewBag.Cultures = Cultures;
            ViewBag.Translation = _model.Translation ?? "";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<string> UploadFile(IFormFile file, string selectedLang)
        {
            //TODO: Validation
            return await _model.TranslateUploadedFile(file, selectedLang);
        }
        [HttpPost]
        public async Task<string> UploadUrl(string url, string selectedLang)
        {
            string text = "Test";
            new DirectoryInfo(_environment.WebRootPath).CreateSubdirectory("Files");
            var uri = new Uri(url);
            var response = await _httpClient.GetAsync(uri);
            if (!response.Content.Headers.ContentDisposition?.FileName?.EndsWith(".docx") ?? true)
                return Resources.Views.Home.Index.NotDocxOrNull;
            return await _model.TranslateUrlFile(response, selectedLang);
        }

        public IActionResult ChangeCulture(string lang)
        {
            var cookeVal = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang));
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, cookeVal);
            CultureInfo.CurrentCulture = new CultureInfo(lang);
            return RedirectToAction("Index");
        }
    }
}