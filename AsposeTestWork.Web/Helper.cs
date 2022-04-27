using System.Globalization;
using System.Resources;

namespace AsposeTestWork.Web;

public class Helper
{
    public static IEnumerable<CultureInfo> GetAvailableCultures()
    {
        List<CultureInfo> result = new List<CultureInfo>();
        ResourceManager rm = AsposeTestWork.Web.Resources.Views.Home.Index.ResourceManager;
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (var culture in cultures)
        {
            if(culture.Equals(CultureInfo.InvariantCulture)) continue;
            var rs = rm.GetResourceSet(culture, true, false);
            if(rs is not null)
                result.Add(culture);
        }
        return result;
    }
}