using System;
using Microsoft.AspNetCore.Mvc;
using Homeworks.BusinessLogic.Interface;
using Homeworks.BusinessLogic.Extensions;
using Homeworks.WebApi.Models;
using Homeworks.Domain;

namespace Homeworks.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeworksController : ControllerBase
    {
        private ILogic<Homework> homeworks;

        public HomeworksController(ILogic<Homework> homeworks) : base()
        {
            this.homeworks = homeworks;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(HomeworkModel.ToModel(homeworks.GetAll()));
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(Guid id)
        {
            var homework = homeworks.Get(id);
            if (homework == null) {
                return NotFound();
            }
            return Ok(HomeworkModel.ToModel(homework));
        }

        [HttpPost("{id}/Exercises", Name = "AddExercise")]
        public IActionResult PostExercise(Guid id, [FromBody]ExerciseModel exercise)
        {
            var newExercise = homeworks.AddExercise(id, ExerciseModel.ToEntity(exercise));
            if (newExercise == null) {
                return BadRequest();
            }
            return CreatedAtRoute("GetExercise", new { id = newExercise.Id }, ExerciseModel.ToModel(newExercise));
        }

        [HttpPost]
        public IActionResult Post([FromBody]HomeworkModel model)
        {
            try {
                var homework = homeworks.Create(HomeworkModel.ToEntity(model));
                return CreatedAtRoute("Get", new { id = homework.Id }, HomeworkModel.ToModel(homework));
            } catch(ArgumentException e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]HomeworkModel model)
        {
            try {
                var homework = homeworks.Update(id, HomeworkModel.ToEntity(model));
                return CreatedAtRoute("Get", new { id = homework.Id }, HomeworkModel.ToModel(homework));
            } catch(ArgumentException e) {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            homeworks.Remove(id);
            return NoContent();
        }

    }
}
