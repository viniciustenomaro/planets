using TesteApi.DataAccess.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace TesteApi.DataAccess
{
    public class Context : DbContext, IDatabaseContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var item in DataAccessDataConfigurations.Instance.Configurations())
            {
                modelBuilder.ApplyConfiguration(item);
            }
        }
    }
}
