using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.Domain;

namespace Homeworks.WebApi.Models
{
    public class HomeworkModel : Model<Homework, HomeworkModel>
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int Score { get; set; }
        public List<ExerciseModel> Exercises {get; set;}

        public HomeworkModel()
        {
            Exercises = new List<ExerciseModel>();
        }

        public HomeworkModel(Homework entity)
        {
            SetModel(entity);
        }

        public override Homework ToEntity() => new Homework()
        {
            Id = this.Id,
            Description = this.Description,
            DueDate = this.DueDate,
            Exercises = this.Exercises.ConvertAll(m => m.ToEntity()),
        };

        protected override HomeworkModel SetModel(Homework entity)
        {
            Id = entity.Id;
            Description = entity.Description;
            DueDate = entity.DueDate;
            Score = entity.Exercises.Sum(x => x.Score);
            Exercises = entity.Exercises.ConvertAll(m => new ExerciseModel(m));
            return this;
        }

    }
}
