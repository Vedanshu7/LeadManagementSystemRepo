using System.Web.Mvc;
using LMS.Web.BAL.Helper;
using LMS.Web.BAL.Interface;
using LMS.Web.BAL.Manager;
using Unity;
using Unity.Mvc5;

namespace LMS.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ILoginManager, LoginManager>();
            container.RegisterType<IUserManager, UserManager>();
            container.RegisterType<ILeadManager, LeadManager>();
            container.RegisterType<IModelManager, ModelManager>();
            container.RegisterType<IBrandManager, BrandManager>();
            container.AddNewExtension<UnityRepositoryHelper>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}