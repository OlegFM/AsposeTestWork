using Aspose.Words;
using Aspose.Words.Notes;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AsposeTestWork.Core
{
    public class WordProcessor
    {
        private Document _file;
        private readonly IConfiguration _configuration;
        private DocumentBuilder _docBuilder;
        private CultureInfo _translationLang;
        private CultureInfo[] _possibleTranslationLangs;
        private Translaters.ITranslateProvider _translater;

        public WordProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
            _translater = new Translaters.Yandex.Translater(_configuration);
        }
        public void ReadWordFile(string path)
        {
            _file = new Document(path);
            _docBuilder = new DocumentBuilder();
        }

        public void SetTranslationLang(CultureInfo culture)
        {
            _translationLang = culture;
        }

        public string Translate()
        {
            CultureInfo detectedLang;
            string text = AsposeTestProcessing();
            detectedLang = _translater.DetectLanguage(text);
            _possibleTranslationLangs = _translater.GetSupportedLanguages();
            StringBuilder sb = new StringBuilder();
            foreach (var translate in _translater.Translate(new string[] { text }, detectedLang, _translationLang))
            {
                sb.AppendLine(translate);
            }
            return sb.ToString();
        }
        public string AsposeTestProcessing()
        {
            string response = HeaderFooterProcessing(_file);
            if(!string.IsNullOrEmpty(response.Trim(new char[] { '\r', '\n' })))
                return response;
            response = NotesProcessing(_file);
            if (!string.IsNullOrEmpty(response.Trim(new char[] { '\r', '\n' })))
                return response;
            response = ParagraphProcessing(_file);
            if (!string.IsNullOrEmpty(response.Trim(new char[] { '\r', '\n' })))
                return response;
            return "Empty document";
        }

        private string ParagraphProcessing(Document document)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Paragraph paragraph in document.FirstSection.Body.Paragraphs)
            {
                if (paragraph.GetText().Contains("Evaluation Only. Created with Aspose.Words"))
                    continue;
                sb.AppendLine(paragraph.GetText().Trim('\f') + Environment.NewLine);
            }
            return sb.ToString();
        }

        public string NotesProcessing(Document document)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Paragraph paragraph in document.FirstSection.Body)
            {
                var footnotes = paragraph.Where(n => n.NodeType == NodeType.Footnote);
                foreach(Footnote footnote in footnotes)
                {
                    sb.AppendLine(footnote.GetText() + Environment.NewLine);
                }
            }
            return sb.ToString();
        }
        public string HeaderFooterProcessing(Document document)
        {
            StringBuilder sb = new StringBuilder();
            HeaderFooterCollection hederfooters = document.FirstSection.HeadersFooters;
            HeaderFooter header = hederfooters[HeaderFooterType.HeaderPrimary];
            CollectHeaderFooterText(header, ref sb);
            HeaderFooter footer = hederfooters[HeaderFooterType.FooterPrimary];
            CollectHeaderFooterText(footer, ref sb);
            return sb.ToString();
        }
        public void CollectHeaderFooterText(in HeaderFooter node, ref StringBuilder sb)
        {
            foreach (Paragraph paragraph in node.Paragraphs)
            {
                if (paragraph.GetText().Contains("Created with an evaluation copy of Aspose.Words."))
                    continue;
                sb.AppendLine(paragraph.GetText() + Environment.NewLine);
            }
        }
    }
}