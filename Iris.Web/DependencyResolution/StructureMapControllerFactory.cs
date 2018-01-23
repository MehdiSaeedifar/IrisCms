using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Iris.Web.DependencyResolution
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, $"Resource not found : {requestContext.HttpContext.Request.Path}");
            }
            return SmObjectFactory.Container.GetInstance(controllerType) as Controller;
        }
    }
}