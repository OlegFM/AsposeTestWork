// See https://aka.ms/new-console-template for more information
using AsposeTestWork.Core;
using System.Globalization;

string filePath = args[0];
string lang = args[1];
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
    if (filePath is null)
    {
        Console.WriteLine("Please, enter the path to Word file");
        filePath = Console.ReadLine();
    }
    if (!File.Exists(filePath))
    {
        Console.WriteLine("File not exists.");
        filePath = null;
        CheckPathToFile();
    }
}

void CheckLang()
{
    if (lang is null)
    {
        Console.WriteLine("Please, enter the language in format **-**");
        lang = Console.ReadLine();      
    }
    culture = cultures.FirstOrDefault(c => c.Name == lang);
    if (culture is null)
    {
        Console.WriteLine("Wrong, usupported culture.");
        CheckLang();
    }
}