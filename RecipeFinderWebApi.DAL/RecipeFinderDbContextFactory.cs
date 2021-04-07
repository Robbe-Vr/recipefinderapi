using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.DAL
{
    public class RecipeFinderDbContextFactory : IDesignTimeDbContextFactory<RecipeFinderDbContext>
    {
        public RecipeFinderDbContext CreateDbContext(string[] args)
        {
            AppConfiguration appConfig = new AppConfiguration();
            var opsBuilder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
            opsBuilder.UseSqlServer(appConfig.sqlConnectionString);
            return new RecipeFinderDbContext(opsBuilder.Options);
        }
    }

}
