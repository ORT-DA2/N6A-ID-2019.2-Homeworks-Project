using Microsoft.EntityFrameworkCore;

namespace Homeworks.DataAccess 
{
    public class ContextFactory
    {
        public static HomeworksContext GetMemoryContext(string nameBd) {
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            return new HomeworksContext(GetMemoryConfig(builder, nameBd));
        }

        private static DbContextOptions GetMemoryConfig(DbContextOptionsBuilder builder, string nameBd) {
            builder.UseInMemoryDatabase(nameBd);
            return builder.Options;
        }

        public static HomeworksContext GetSqlContext() {
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            return new HomeworksContext(GetSqlConfig(builder));
        }

        private static DbContextOptions GetSqlConfig(DbContextOptionsBuilder builder) {
            builder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=HomeworksDB;
                Trusted_Connection=True;MultipleActiveResultSets=True;");
            return builder.Options;
        }
    }
}