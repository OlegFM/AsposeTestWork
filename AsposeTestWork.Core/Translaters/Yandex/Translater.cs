using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AsposeTestWork.Core.Translaters.Yandex
{
    public class Translater : ITranslateProvider, IDisposable
    {
        private string _apiKey = "AQVNxMXvH8_ReOgBuzPr5cwN05qgBg__ypCauo66";
        private const string _baseUri = "https://translate.api.cloud.yandex.net/translate/v2/";
        private readonly string _folderId;
        private HttpClient _httpClient;

        public Translater(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration.GetSection("Translater:Yandex:ApiKey").Value;
            _folderId = configuration.GetSection("Translater:Yandex:FolderId").Value;
            _httpClient.BaseAddress = new Uri(_baseUri);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Api-Key {_apiKey}");
        }
        public CultureInfo DetectLanguage(string text)
        {
            var detectResponse = new DetectResponse();
            var detectRequest = new DetectRequest()
            {
                text = text,
                folderId = _folderId
            };
            var content = new StringContent(JsonSerializer.Serialize(detectRequest), Encoding.UTF8, "application/json");
            var result = _httpClient.PostAsync("detect", content).Result;
            result.EnsureSuccessStatusCode();
            Stream resultContent = result.Content.ReadAsStream();
            if (resultContent is not null)
                detectResponse = JsonSerializer.Deserialize<DetectResponse>(resultContent);
            return new CultureInfo(detectResponse?.languageCode ?? CultureInfo.InvariantCulture.Name);
        }

        public CultureInfo[] GetSupportedLanguages()
        {
            ListLanguagesResponse? listLanguagesResponse = new ListLanguagesResponse();
            ListLanguagesRequest listLanguagesRequest = new ListLanguagesRequest()
            {
                folderId = _folderId
            };
            var content = new StringContent(JsonSerializer.Serialize(listLanguagesRequest), Encoding.UTF8, "application/json");
            var result = _httpClient.PostAsync("languages", content).Result;
            result.EnsureSuccessStatusCode();
            Stream resultContent = result.Content.ReadAsStream();
            if(resultContent is not null)
                listLanguagesResponse = JsonSerializer.Deserialize<ListLanguagesResponse>(resultContent);

            return listLanguagesResponse?.languages.Select(language => { return new CultureInfo(language.code); }).ToArray() ?? new CultureInfo[0];
        }

        public string[] Translate(string[] texts, CultureInfo? translateFrom, CultureInfo translateTo)
        {
            TranslateResponse? translateResponse = new TranslateResponse();
            TranslateRequest translateRequest = new TranslateRequest()
            {
                folderId = _folderId,
                texts = texts.ToList<string>(),
                targetLanguageCode = translateTo.TwoLetterISOLanguageName,
                sourceLanguageCode = translateFrom?.TwoLetterISOLanguageName ?? ""
            };
            var content = new StringContent(JsonSerializer.Serialize(translateRequest), Encoding.UTF8, "application/json");
            var result = _httpClient.PostAsync("translate", content).Result;
            result.EnsureSuccessStatusCode();
            Stream resultContent = result.Content.ReadAsStream();
            if(resultContent is not null)
                translateResponse = JsonSerializer.Deserialize<TranslateResponse>(resultContent);

            return translateResponse?.translations.Select(t => t.text).ToArray() ?? new string[0];
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
