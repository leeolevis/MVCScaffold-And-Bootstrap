using System.Web;
using System.Web.Mvc;
using WebApp4.Infrastructure;

namespace WebApp4
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var logger = DependencyResolver.Current.GetService<ILogger>();
            filters.Add(new NLogMvcHandleErrorAttribute(logger));
            //filters.Add(new HandleErrorAttribute());
        }
    }
}