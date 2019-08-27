using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.Domain;
using Microsoft.EntityFrameworkCore;

namespace Homeworks.DataAccess
{
    public class HomeworkRepository : BaseRepository<Homework>
    {
        public HomeworkRepository(DbContext context)
        {
            Context = context;
        }

        public override Homework Get(Guid id)
        {
            return Context.Set<Homework>().Include("Exercises")
                .First(x => x.Id == id);
        }

        public override IEnumerable<Homework> GetAll()
        {
            return Context.Set<Homework>().Include("Exercises")
                .ToList();
        }
    }
}