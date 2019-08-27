using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.Domain;
using Microsoft.EntityFrameworkCore;

namespace Homeworks.DataAccess
{
    public class ExerciseRepository : BaseRepository<Exercise>
    {
        public ExerciseRepository(DbContext context)
        {
            Context = context;
        }

        public override Exercise Get(Guid id)
        {
            return Context.Set<Exercise>().First(x => x.Id == id);
        }

        public override IEnumerable<Exercise> GetAll()
        {
            return Context.Set<Exercise>().ToList();
        }
    }
}