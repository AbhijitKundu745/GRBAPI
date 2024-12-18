﻿using PSL.Laundry.CentralService.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PSL.Laundry.CentralService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            GlobalConfiguration.Configuration.Filters.Add(new CustomExceptionFilter());
            //config.Filters.Add((IExceptionFilter)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(CustomExceptionFilter)));

            // Web API configuration and services

            //var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            //config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
            name: "TransactionApproval",
            routeTemplate: "TransactionApproval",
            defaults: new { controller = "TransactionApproval", TransactionNumber = RouteParameter.Optional }
            );


            config.Routes.MapHttpRoute(
            name: "TripDetails",
            routeTemplate: "TripDetails",
            defaults: new { controller = "TripDetails", TransactionNumber = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
         name: "TransactionDetails",
         routeTemplate: "TransactionDetails",
         defaults: new { controller = "TransactionDetails", TransactionNumber = RouteParameter.Optional }
         );

            config.Routes.MapHttpRoute(
             name: "GetApprovalTransaction",
             routeTemplate: "GetApprovalTransaction",
             defaults: new { controller = "TransactionApproval", Action = "GetByTruckID", TransactionNumber = RouteParameter.Optional }
             );




            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
