using System;
using Homeworks.BusinessLogic.Interface;
using Homeworks.Domain;
using Homeworks.WebApi.Controllers;
using Homeworks.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Homeworks.WebApi.Tests
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public void CreateValidUserTest()
        {
            var user = new UserModel
            {
                UserName = "Hola",
                Password = "Hola"
            };
            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Create(It.IsAny<User>())).Returns(user.ToEntity());
            var controller = new UsersController(mock.Object);

            var result = controller.Post(user);
            var createdResult = result as CreatedAtRouteResult;
            var model = createdResult.Value as UserModel;

            mock.VerifyAll();
            Assert.AreEqual(user.UserName, model.UserName);
        }

        [TestMethod]
        public void CreateInvalidUserTest()
        {
            var mock = new Mock<ILogic<User>>(MockBehavior.Strict);
            mock.Setup(m => m.Create(null)).Throws(new ArgumentException());
            var controller = new UsersController(mock.Object);

            var result = controller.Post(null);

            mock.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
