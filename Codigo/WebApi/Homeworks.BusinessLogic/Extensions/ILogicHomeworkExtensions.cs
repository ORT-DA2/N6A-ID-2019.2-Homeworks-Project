using Homeworks.BusinessLogic.Interface;
using Homeworks.Domain;
using System;

namespace Homeworks.BusinessLogic.Extensions
{
    public static class ILogicHomeworkExtensions
    {
        public static Exercise AddExercise(this ILogic<Homework> logic, Guid id, Exercise exercise)
        {
            Homework homework = logic.Get(id);
            homework.Exercises.Add(exercise);
            logic.Update(id, homework);
            return exercise;
        }
    }
}
