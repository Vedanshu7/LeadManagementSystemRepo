using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LMS.Web.Controllers;
using System.Web.Mvc;
using LMS.Web.BAL.Manager;
using LMS.Web.BAL.ViewModels;
using LMS.Web.UnitTests.Repository;

namespace LMS.Web.UnitTests
{
    [TestClass]
    public class AuthenticationControllerTest
    {
        [TestMethod]
        public void Login_ShouldReturnSuccess()
        {
            var controller = new AuthenticationController(new LoginManager(new TestLoginRepository()));

            LoginViewModel testUser = new LoginViewModel() { Email = "test@test.com", Password = "Lms@2021", Role = 1 };

            var result = controller.Login(testUser);

            Assert.AreEqual("Success", result);
        }
    }
}
