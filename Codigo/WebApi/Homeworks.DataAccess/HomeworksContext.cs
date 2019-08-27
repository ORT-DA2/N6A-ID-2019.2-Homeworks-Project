using Homeworks.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homeworks.DataAccess
{
    public class HomeworksContext : DbContext
    {
        public DbSet<Homework> Homeworks {get; set;}
        public DbSet<Exercise> Exercises {get; set;}
        public DbSet<User> Users {get; set;}

        public HomeworksContext(DbContextOptions options) : base(options)
        {
            
        }

    }
}