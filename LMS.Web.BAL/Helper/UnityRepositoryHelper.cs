using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Web.BAL.Interface;
using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Repository;
using Unity.Events;
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
            Container.RegisterType<ISalesLeadRepository, SalesLeadRepository>();

        }
    }
}
