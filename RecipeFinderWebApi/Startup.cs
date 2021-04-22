using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RecipeFinderWabApi.Logic.Handlers;
using RecipeFinderWabApi.Logic.signInHandlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Management;
using RecipeFinderWebApi.Logic.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            services.AddMvc();

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

            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new UserActionHandler(new UserActionRepo(context), new UserRepo(context)); });
            services.AddScoped(x => { RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions); return new SignInHandler(new UserRepo(context), new RoleRepo(context)); });
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

            app.UseStaticFiles();

            app.UseCors(RFCorsPolicy);

            app.UseAuthorization();

            app.Use((HttpContext context, Func<Task> next) =>
            {
                var dbContext = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions);

                UserHandler userHandler = new UserHandler(new UserRepo(dbContext), new UserRoleRelationRepo(dbContext), new KitchenRepo(dbContext));

                RequestInfoManager.LogUserAction = new Func<int>(() =>
                {
                    if (RequestInfoManager.IsCompleted)
                    {
                        UserActionHandler handler = new UserActionHandler(new UserActionRepo(dbContext), new UserRepo(dbContext));

                        return handler.Create(RequestInfoManager.Action);
                    }

                    return 0;
                });

                Func<Stream, object> StreamToObject = (Stream stream) =>
                {
                    try
                    {
                        if (!stream.CanRead) return new object { };

                        byte[] buffer = new byte[context.Response.Body.Length];
                        context.Response.Body.Read(buffer, 0, buffer.Length);

                        return JsonConvert.DeserializeObject(Convert.ToBase64String(buffer));
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                        return new object { };
                    }
                };

                if (context.Request.Path.StartsWithSegments("/api") && context.Request.Headers.ContainsKey("RecipeFinder_AccessToken"))
                {
                    string userId = TokenManager.GetUserIdByToken(context.Request.Headers["RecipeFinder_AccessToken"]);
                    if (!String.IsNullOrEmpty(userId))
                    {
                        User user = userHandler.GetById(userId);
                        if (user != null)
                        {
                            RequestInfoManager.Action.User = user;
                            RequestInfoManager.Action.UserId = user.CountId;

                            RequestInfoManager.Action.Endpoint = context.Request.Path.Value;
                            RequestInfoManager.Action.RequestType = context.Request.Method.ToUpper();
                            RequestInfoManager.Action.ActionPerformedOnTable = context.Request.Path.Value.Split('/')[2];
                            RequestInfoManager.Action.Description = $"User {RequestInfoManager.Action.User.Name}/{RequestInfoManager.Action.UserId} executed a {RequestInfoManager.Action.RequestType} on '{RequestInfoManager.Action.Endpoint}'. " +
                                                 $"Request targeted at table {context.Request.Path.Value.Split('/')[2]}. " +
                                                 $"Request Body: ( {JsonConvert.SerializeObject(StreamToObject(context.Request.Body))} ).";
                        }
                    }
                }

                context.Response.OnCompleted(() =>
                {
                    RequestInfoManager.Action.Success = context.Response.StatusCode >= 200 && context.Response.StatusCode <= 299;

                    object targetObj = context.Request.Method == "GET" ?
                        StreamToObject(context.Response.Body) : StreamToObject(context.Request.Body);
                    RequestInfoManager.Action.RefObject = targetObj;

                    RequestInfoManager.LogUserAction();

                    return Task.CompletedTask;
                });

                return next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
