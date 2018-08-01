using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebServiceTrial1
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WebApiConfig'
    public static class WebApiConfig
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WebApiConfig'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WebApiConfig.Register(HttpConfiguration)'
        public static void Register(HttpConfiguration config)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WebApiConfig.Register(HttpConfiguration)'
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );       
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml"));
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml"));
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain"));
          
        }
    }
}
