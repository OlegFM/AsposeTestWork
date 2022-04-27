using System.Globalization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace AsposeTestWork.Web.Filters;

public class CultureAttribute : ActionFilterAttribute, IActionFilter
{
    public new void OnActionExecuted(ActionExecutedContext filterContext)
    {
        string cultureName = null;
        // Получаем куки из контекста, которые могут содержать установленную культуру
        var cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];
        if (cultureCookie != null)
            cultureName = cultureCookie;
        else
            cultureName = "ru"; 
 
        // Список культур
        List<string> cultures = new List<string>() {"ru", "en", "de"};
        if (!cultures.Contains(cultureName))
        {
            cultureName = "ru";
        }
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
    }
}