using System;
using System.Collections.Generic;
using Homeworks.Domain;
using Homeworks.BusinessLogic.Interface;
using Homeworks.DataAccess.Interface;

namespace Homeworks.BusinessLogic
{
    public class HomeworkLogic : ILogic<Homework>
    {
        private IRepository<Homework> repositoryHome;

        public HomeworkLogic(IRepository<Homework> homeworks) {
            repositoryHome = homeworks;
        }

        public Homework Create(Homework homework) {
            ThrowErrorIfItsInvalid(homework);
            repositoryHome.Add(homework);
            repositoryHome.Save();
            return homework;
        }

        public void Remove(Guid id) {
            Homework homework = repositoryHome.Get(id);
            ThrowErrorIfItsNull(homework);
            repositoryHome.Remove(homework);
            repositoryHome.Save();
        }

        public Homework Update(Guid id, Homework homework) {
            Homework homeworkToUpdate = repositoryHome.Get(id);
            ThrowErrorIfItsNull(homeworkToUpdate);
            homeworkToUpdate.Update(homework);
            repositoryHome.Update(homeworkToUpdate);
            repositoryHome.Save();
            return homeworkToUpdate;
        }

        public Exercise AddExercise(Guid id, Exercise exercise)
        {
            Homework homework = repositoryHome.Get(id);
            ThrowErrorIfItsNull(homework);
            homework.Exercises.Add(exercise);
            repositoryHome.Update(homework);
            repositoryHome.Save();
            return exercise;
        }

        public Homework Get(Guid id) 
        {
            return repositoryHome.Get(id);
        }

        public IEnumerable<Homework> GetAll() 
        {
            return repositoryHome.GetAll();
        }

        private static void ThrowErrorIfItsNull(Homework homework)
        {
            if (homework == null)
            {
                throw new ArgumentException("Invalid guid");
            }
        }

        private void ThrowErrorIfItsInvalid(Homework homework) 
        {
            if (!homework.IsValid()) 
            {
                throw new ArgumentException("Lanza error por que es invaldia la entity");
            }
        }
    }
}
