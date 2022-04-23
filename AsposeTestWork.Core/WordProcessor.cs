using Aspose.Words;
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
            StringBuilder sb = new StringBuilder();
            _docBuilder.MoveToHeaderFooter(HeaderFooterType.HeaderFirst);
            sb.Append(_docBuilder.Document.GetText());
            return sb.ToString();
        }
    }
}