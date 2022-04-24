// See https://aka.ms/new-console-template for more information
using AsposeTestWork.Core;
using System.Globalization;

string filePath = null;
string lang = null;
CultureInfo culture;
CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);


CheckPathToFile();
CheckLang();

WordProcessor processor = new WordProcessor();
processor.ReadWordFile(filePath);
processor.SetTranslationLang(culture);
Console.WriteLine(processor.AsposeTestProcessing());


void CheckPathToFile()
{
    if(args.Length > 0)
        filePath = args[0];
    if (filePath is null)
    {
        Console.WriteLine("Please, enter the path to Word file");
        filePath = Console.ReadLine();
        if (string.IsNullOrEmpty(filePath))
        {
            Console.WriteLine("Filename is null.");
            filePath = null;
            CheckPathToFile();
            return;
        }
    }
    if (!File.Exists(Path.GetFullPath(filePath)))
    {
        Console.WriteLine("File not exists.");
        filePath = null;
        CheckPathToFile();
        return;
    }
    filePath = Path.GetFullPath(filePath);
}

void CheckLang()
{
    if (args.Length > 1)
        lang = args[1];
    if (lang is null)
    {
        Console.WriteLine("Please, enter the language in format **-**");
        lang = Console.ReadLine();      
    }
    culture = cultures.FirstOrDefault(c => c.Name == lang);
    if (culture is null)
    {
        Console.WriteLine("Wrong, usupported culture.");
        lang = null;
        CheckLang();
    }
}