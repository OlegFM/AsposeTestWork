using Aspose.Words;
using Aspose.Words.Notes;
using System.Globalization;
using System.Text;

namespace AsposeTestWork.Core
{
    public class WordProcessor
    {
        private Document _file;
        private DocumentBuilder _docBuilder;
        private CultureInfo _translationLang;
        public void ReadWordFile(string path)
        {
            _file = new Document(path);
            _docBuilder = new DocumentBuilder();
        }

        public void SetTranslationLang(CultureInfo culture)
        {
            _translationLang = culture;
        }

        public void Translate()
        {

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