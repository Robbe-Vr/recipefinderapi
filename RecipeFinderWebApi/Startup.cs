using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using RecipeFinderWabApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private string RFCorsPolicy = "RFCorsPolicy";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: RFCorsPolicy,
                    builder =>
                    {
                        builder//.WithOrigins("http://localhost:3000")
                                            .AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "_af_recipefinder";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                options.HeaderName = "X-XSRF-TOKEN";
            });


            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new RecipeHandler(new RecipeRepo(context), new RecipeCategoryRelationRepo(context), new RequirementsListRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new IngredientCategoryHandler(new IngredientCategoryRepo(context), new IngredientCategoryRelationRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new RecipeCategoryHandler(new RecipeCategoryRepo(context), new RecipeCategoryRelationRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new UserHandler(new UserRepo(context), new UserRoleRelationRepo(context), new KitchenRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new RoleHandler(new RoleRepo(context), new UserRoleRelationRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new KitchenHandler(new KitchenRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new RequirementsListHandler(new RequirementsListRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new GroceryListHandler(new GroceryListRepo(context)); });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
