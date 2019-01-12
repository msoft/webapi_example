using System;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace WebApiExample
{
    public class CustomRouter : IRouter
    {
        private IRouter _defaultRouter;
    
        public CustomRouter(IRouter defaultRouter)
        {
            _defaultRouter = defaultRouter;
        }
    
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return _defaultRouter.GetVirtualPath(context);
        }
    
        public async Task RouteAsync(RouteContext context)
        {
            var path = context.HttpContext.Request.Path.Value;
            
            if (path.Contains("admin"))
            {
                context.RouteData.Values["controller"] = "Admin";
                context.RouteData.Values["action"] = "GetOrders";
    
                await _defaultRouter.RouteAsync(context);
            }
        }
    }
}