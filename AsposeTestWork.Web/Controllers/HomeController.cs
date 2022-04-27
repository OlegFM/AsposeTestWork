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
        private HomeViewModel _model = new HomeViewModel();

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _logger = logger;
            _environment = hostEnvironment;
            _configuration = configuration;
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
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                new DirectoryInfo(_environment.WebRootPath).CreateSubdirectory("Files");
                  
                string path = "/Files/" + Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var processor = new WordProcessor(_configuration);
                processor.ReadWordFile(_environment.WebRootPath + path);
                processor.SetTranslationLang(CultureInfo.CurrentCulture);
                _model.Translation = processor.Translate();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ChangeCulture(string lang)
        {
            string returnUrl = "https://localhost:7081/";
            var cookeVal = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang));
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, cookeVal);
            CultureInfo.CurrentCulture = new CultureInfo(lang);
            return Redirect(returnUrl);
        }
    }
}