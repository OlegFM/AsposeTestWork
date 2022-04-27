using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeTestWork.Core.Translaters
{
    internal interface ITranslateProvider
    {
        public string[] Translate (string[] text, CultureInfo? translateFrom, CultureInfo translateTo);
        public CultureInfo[] GetSupportedLanguages();
        public CultureInfo DetectLanguage(string text);
    }
}
