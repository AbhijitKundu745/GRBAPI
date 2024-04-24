using PSL.Laundry.CentralService.DataAccessLayer;

using PSL.Warehouse.CentralService.IDataAccessLayer;
using PSL.Laundry.CentralService.Logger;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using PSL.Warehouse.CentralService.Logger;
using PSL.Laundry.CentralService.Filters;

namespace PSL.Warehouse.CentralService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            //Logger
            container.RegisterType(typeof(ILogger<>), typeof(Log4netLogger<>));
            container.RegisterType<IDataAccess, DataAccess>();
            container.RegisterType<IPDADAL, PDADAL>();
            container.RegisterType<CustomExceptionFilter>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}