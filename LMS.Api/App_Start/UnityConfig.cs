using LMS.Api.BAL.Helper;
using LMS.Api.BAL.Interface;
using LMS.Api.BAL.Manager;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;


namespace LMS.Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ILeadManager, LeadManager>();
            container.AddNewExtension<UnityRepositoryHelper>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}