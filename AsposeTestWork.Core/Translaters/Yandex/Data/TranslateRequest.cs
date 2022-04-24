using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeTestWork.Core.Translaters.Yandex
{
    public class GlossaryPair
    {
        public string sourceText { get; set; }
        public string translatedText { get; set; }
    }

    public class GlossaryData
    {
        public List<GlossaryPair> glossaryPairs { get; set; }
    }

    public class GlossaryConfig
    {
        public GlossaryData glossaryData { get; set; }
    }

    public class TranslateRequest
    {
        public string sourceLanguageCode { get; set; }
        public string targetLanguageCode { get; set; }
        public string format { get; set; }
        public List<string> texts { get; set; }
        public string folderId { get; set; }
        public string model { get; set; }
        public GlossaryConfig glossaryConfig { get; set; }
    }

}
