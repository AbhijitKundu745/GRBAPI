using PSL.Laundry.CentralService.Logger;
using PSL.Laundry.CentralService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace PSL.Laundry.CentralService.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        //ILogger<CustomExceptionFilter> logger;
        //public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        //{
        //    this.logger = logger;
        //}
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = string.Empty;
            if (actionExecutedContext.Exception.InnerException == null)
                exceptionMessage = actionExecutedContext.Exception.Message;
            else
                exceptionMessage = actionExecutedContext.Exception.InnerException.Message;

            string controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            if (actionExecutedContext.Exception is ArgumentException)
            {

            }
            else
            {

            }

            var res = new Response()
            {
                message = exceptionMessage,
                status = false
            };
            //ILogger<TripDetailsController> logger
            //We can log this exception message to the file or database.  
            var response = new HttpResponseMessage()
            {
                ReasonPhrase = "Internal Server Error.Please Contact your Administrator.",
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("An unhandled exception was thrown by service"),

            };
            
           // actionExecutedContext.Response = response;
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, res); 
        }
    }
}