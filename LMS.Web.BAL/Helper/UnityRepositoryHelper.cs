using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Repository;
using Unity.Extension;
using Unity;

namespace LMS.Web.BAL.Helper
{
    public class UnityRepositoryHelper : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<ILoginRepository, LoginRepository>();
            Container.RegisterType<IUserRepository, UserRepository>();
            Container.RegisterType<ILeadRepository, LeadRepository>();
            Container.RegisterType<IModelRepository, ModelRepository>();
            Container.RegisterType<IBrandRepository, BrandRepository>();
            Container.RegisterType<IDealerRepository, DealerRepository>();
        }
    }
}
