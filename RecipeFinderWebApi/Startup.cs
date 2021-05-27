using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RecipeFinderWebApi.Logic.Handlers;
using RecipeFinderWebApi.DAL;
using RecipeFinderWebApi.DAL.Repositories;
using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Management;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using IdentityServer4.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RecipeFinderWebApi.UI.Auth;
using RecipeFinderWebApi.Logic;

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
                        builder.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://192.168.2.29:3000", "https://192.168.2.29:3000", "https://recipefinder.sywapps.com")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            services.AddMvc();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            services.AddHttpContextAccessor();

            Func<HttpContext, User> getUserByToken = new Func<HttpContext, User>((HttpContext context) =>
            {
                string accessToken = context.Request.Headers["RecipeFinder_AccessToken"];

                return AuthManager.GetUser(accessToken);
            });

            services.AddScoped(x => {
                RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions);
                RecipeHandler recipeHandler = null;
                KitchenHandler kitchenHandler = new KitchenHandler(new KitchenRepo(context));
                UnitTypeHandler unitTypeHandler = new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context));
                PreparableRecipesAlgorithm preparableAlgorithm = new PreparableRecipesAlgorithm(recipeHandler, kitchenHandler, unitTypeHandler);
                recipeHandler = new RecipeHandler(
                    new RecipeRepo(context, getUserByToken(x.GetService<IHttpContextAccessor>().HttpContext)),
                    new RecipeCategoryRelationRepo(context),
                    new RequirementsListRepo(context),
                    preparableAlgorithm);
                WhatToBuyAlgorithm algorithm = new WhatToBuyAlgorithm(recipeHandler, kitchenHandler, unitTypeHandler);
                return new IngredientHandler(new IngredientRepo(context), new IngredientCategoryRelationRepo(context), new IngredientUnitTypeRelationRepo(context), algorithm);
            });
            services.AddScoped(x => {
                RecipeFinderDbContext context = new RecipeFinderDbContext(RecipeFinderDbContext.ops.dbOptions);
                RecipeHandler recipeHandler = null;
                PreparableRecipesAlgorithm algorithm = new PreparableRecipesAlgorithm(recipeHandler, new KitchenHandler(new KitchenRepo(context)), new UnitTypeHandler(new UnitTypeRepo(context), new IngredientUnitTypeRelationRepo(context)));
                recipeHandler = new RecipeHandler(
                    new RecipeRepo(context, getUserByToken(x.GetService<IHttpContextAccessor>().HttpContext)),
                    new RecipeCategoryRelationRepo(context),
                    new RequirementsListRepo(context),
                    algorithm);
                return recipeHandler;
            });
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

            

            app.Use((HttpContext context, Func<Task> next) =>
            {
                return next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void tokenValidated(HttpContext context)
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
                string userId = "";
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
        }
    }
}
