using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Homeworks.DataAccess;
using Homeworks.DataAccess.Interface;
using Homeworks.Domain;
using System.Linq;

namespace Homeworks.DataAccess.Tests
{
    [TestClass]
    public class HomeworkRepositoryTest
    {
        [TestMethod]
        public void TestHomeworkAddOK()
        {
            var context = ContextFactory.GetMemoryContext("MemoriaTestDB");
            HomeworkRepository homeworkRepo = new HomeworkRepository(context);

            var name = "una tarea";

            homeworkRepo.Add(new Homework {
                Id = Guid.NewGuid(),
                Description = name,
            });
            homeworkRepo.Save();
            var homeworks = homeworkRepo.GetAll().ToList();

            Assert.AreEqual(homeworks[0].Description, name);
        }

        [TestMethod]
        public void TestHomeworkAddOK2()
        {
            var context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            HomeworkRepository homeworkRepo = new HomeworkRepository(context);

            var name = "una tarea";

            homeworkRepo.Add(new Homework {
                Id = Guid.NewGuid(),
                Description = name,
            });
            homeworkRepo.Save();
            var homeworks = homeworkRepo.GetAll().ToList();

            Assert.AreEqual(homeworks.Count, 1);
        }
    }
}
