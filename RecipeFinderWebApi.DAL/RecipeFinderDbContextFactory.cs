using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.DAL
{
    public class RecipeFinderDbContextFactory : IDesignTimeDbContextFactory<RecipeFinderDBContext>
    {
        public RecipeFinderDBContext CreateDbContext(string[] args)
        {
            AppConfiguration appConfig = new AppConfiguration();
            var opsBuilder = new DbContextOptionsBuilder<RecipeFinderDBContext>();
            opsBuilder.UseSqlServer(appConfig.sqlConnectionString);
            return new RecipeFinderDBContext(opsBuilder.Options);
        }
    }

}
