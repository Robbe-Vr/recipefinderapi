using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecipeFinderWabApi.Logic.Handlers;
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
                        builder.WithOrigins("http://localhost:3000")
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
                });

            services.AddScoped(x => new IngredientHandler(new IngredientRepo()));
            services.AddScoped(x => new RecipeHandler(new RecipeRepo()));
            services.AddScoped(x => new IngredientCategoryHandler(new IngredientCategoryRepo()));
            services.AddScoped(x => new RecipeCategoryHandler(new RecipeCategoryRepo()));
            services.AddScoped(x => new UnitTypeHandler(new UnitTypeRepo()));
            services.AddScoped(x => new UserHandler(new UserRepo()));
            services.AddScoped(x => new RoleHandler(new RoleRepo()));
            services.AddScoped(x => new KitchenHandler(new KitchenRepo()));
            services.AddScoped(x => new RequirementsListHandler(new RequirementsListRepo()));
            services.AddScoped(x => new GroceryListHandler(new GroceryListRepo()));


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
