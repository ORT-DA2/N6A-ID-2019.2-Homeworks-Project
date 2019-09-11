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
    }
}