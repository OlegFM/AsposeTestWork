using System.Globalization;
using System.Net.Mime;
using AsposeTestWork.Core;

namespace AsposeTestWork.Web.Models;

public class HomeViewModel
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public HomeViewModel(IWebHostEnvironment env, IConfiguration config)
    {
        _environment = env;
        _configuration = config;
    }
    public string Translation { get; set; }

    internal async Task<string> TranslateUploadedFile(IFormFile? file, string selectedLang)
    {
        if (file != null)
        {
            new DirectoryInfo(_environment.WebRootPath).CreateSubdirectory("Files");
            var text = "";
            string path = _environment.WebRootPath + "/Files/" + Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var processor = new WordProcessor(_configuration);
            processor.ReadWordFile(path);
            processor.SetTranslationLang(new CultureInfo(selectedLang));
            text = processor.Translate();
            processor.Close();
            System.IO.File.Delete(path);
            return text;
        }

        return Resources.Views.Home.Index.IsFileNullMessage;
    }

    internal async Task<string> TranslateUrlFile(HttpResponseMessage response, string lang)
    {
        var text = "";
        var filePath = _environment.WebRootPath + "/Files/" + Path.GetRandomFileName() + ".docx";
        using (var rs = new FileStream(filePath, FileMode.CreateNew))
        {
            await response.Content.CopyToAsync(rs);
        }
        var processor = new WordProcessor(_configuration);
        processor.ReadWordFile(filePath);
        processor.SetTranslationLang(new CultureInfo(lang));
        text = processor.Translate();
        processor.Close();
        System.IO.File.Delete(filePath);
        return text;
    }
}