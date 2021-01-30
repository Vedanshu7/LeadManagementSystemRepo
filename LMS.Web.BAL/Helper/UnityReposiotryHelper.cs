using LMS.Web.DAL.Interface;
using LMS.Web.DAL.Repository;
using Unity.Extension;
using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LMS.Web.BAL.Helper
{
    public class UnityReposiotryHelper : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<ILoginRepository,LoginRepository>();
        }
    }
}
