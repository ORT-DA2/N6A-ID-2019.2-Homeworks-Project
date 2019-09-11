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
    public class HomeworksControllerTests
    {
        [TestMethod]
        public void GetHomeworkTest()
        {
            var homeworkModel = new HomeworkModel
            {
                Id = Guid.NewGuid(),
                Description = "A basic homework",
            };
            var mock = new Mock<ILogic<Homework>>(MockBehavior.Strict);
            mock.Setup(m => m.Get(homeworkModel.Id)).Returns(homeworkModel.ToEntity());
            var controller = new HomeworksController(mock.Object);

            var result = controller.Get(homeworkModel.Id);
            var createdResult = result as OkObjectResult;
            var model = createdResult.Value as HomeworkModel;

            mock.VerifyAll();
            Assert.AreEqual(homeworkModel.Description, model.Description);
            Assert.AreEqual(homeworkModel.Id, model.Id);
        }

        [TestMethod]
        public void DeleteHomeworkTest()
        {
            var homeworkGuid = Guid.NewGuid();
            var mock = new Mock<ILogic<Homework>>(MockBehavior.Strict);
            mock.Setup(m => m.Remove(homeworkGuid));
            var controller = new HomeworksController(mock.Object);

            var result = controller.Delete(homeworkGuid);

            mock.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
