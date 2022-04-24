using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsposeTestWork.Core.Translaters.Yandex
{
    public class Language
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class ListLanguagesResponse
    {
        public List<Language> languages { get; set; }
    }

}
