using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeTestWork.Core.Translaters.Yandex
{
    public class DetectRequest
    {
        public string text { get; set; }
        public List<string> languageCodeHints { get; set; }
        public string folderId { get; set; }
    }
}
