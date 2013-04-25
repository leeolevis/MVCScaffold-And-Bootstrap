using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BootstrapMvcSample.Controllers;
using NavigationRoutes;
using WebApp4.Controllers;

namespace BootstrapMvcSample
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapNavigationRoute<HomeController>("首页", c => c.Index());

            routes.MapNavigationRoute<ExampleLayoutsController>("Admin", c => c.Starter())
                  .AddChildRoute<UserController>("个人设置", c => c.Setting())
                  .AddChildRoute<UserController>("密码修改", c => c.Password())
                  .AddChildRoute<UserController>("退出登录", c => c.LogOff())
                ;
        }
    }
}
