using LMS.Api.DAL.Interface;
using LMS.Api.DAL.Repository;
using Unity;
using Unity.Extension;

namespace LMS.Api.BAL.Helper
{
    public class UnityRepositoryHelper : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<ILeadRepository, LeadRepository>();
        }
    }
}
