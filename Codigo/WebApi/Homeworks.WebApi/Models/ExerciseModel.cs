using System;
using System.Collections.Generic;
using Homeworks.Domain;

namespace Homeworks.WebApi.Models
{
    public class ExerciseModel : Model<Exercise, ExerciseModel>
    {
        public Guid Id { get; set; }
        public string Problem { get; set; }
        public int Score { get; set; }

        public ExerciseModel() { }

        public ExerciseModel(Exercise entity)
        {
            SetModel(entity);
        }

        public override Exercise ToEntity() => new Exercise()
        {
            Id = this.Id,
            Problem = this.Problem,
            Score = this.Score,
        };

        protected override ExerciseModel SetModel(Exercise entity)
        {
            Id = entity.Id;
            Problem = entity.Problem;
            Score = entity.Score;
            return this;
        }
    }
}
