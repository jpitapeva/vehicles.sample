using Case.Infra.Data.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Case.Infra.Data.Sql.Design
{
    public class SqlContextFactory : IDesignTimeDbContextFactory<SqlContext>
    {
        public SqlContext CreateDbContext(string[] args)
        {
            string connectionString = @"CHANGE";
            var optionsBuilder = new DbContextOptionsBuilder<SqlContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SqlContext(optionsBuilder.Options);
        }
    }
}