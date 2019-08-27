using System;
using System.Collections.Generic;
using Homeworks.BusinessLogic.Interface;
using Homeworks.DataAccess.Interface;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class ExerciseLogic : ILogic<Exercise>
    {
        private IRepository<Exercise> repository;

        public ExerciseLogic(IRepository<Exercise> repository) 
        {
            this.repository = repository;
        }

        public Exercise Create(Exercise exercise) 
        {
            ThrowErrorIfItsInvalid(exercise);
            repository.Add(exercise);
            repository.Save();
            return exercise;
        }

        public void Remove(Guid id) 
        {
            Exercise exercise = repository.Get(id);
            ThrowErrorIfItsNull(exercise);
            repository.Remove(exercise);
            repository.Save();
        }

        public Exercise Update(Guid id, Exercise exercise)
        {
            ThrowErrorIfItsInvalid(exercise);
            Exercise exerciseToUpdate = repository.Get(id);
            ThrowErrorIfItsNull(exerciseToUpdate);
            exerciseToUpdate.Update(exercise);
            repository.Update(exerciseToUpdate);
            repository.Save();
            return exerciseToUpdate;
        }

        public Exercise Get(Guid id) 
        {
            return repository.Get(id);
        }

        public IEnumerable<Exercise> GetAll() 
        {
            return repository.GetAll();
        }

        private static void ThrowErrorIfItsNull(Exercise exercise)
        {
            if (exercise == null)
            {
                throw new ArgumentException("Invalid guid");
            }
        }

        private void ThrowErrorIfItsInvalid(Exercise exercise) 
        {
            if (!exercise.IsValid()) 
            {
                throw new ArgumentException("Lanza error por que es invaldia la entity");
            }
        }
    }
}
