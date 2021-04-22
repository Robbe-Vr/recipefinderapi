using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.DAL
{
    public class RecipeFinderDbContext : DbContext
    {
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                settings = new AppConfiguration();
                opsBuilder = new DbContextOptionsBuilder<RecipeFinderDbContext>();
                opsBuilder.UseSqlServer(settings.sqlConnectionString);
                dbOptions = opsBuilder.Options;
            }
            public DbContextOptionsBuilder<RecipeFinderDbContext> opsBuilder { get; set; }

            public DbContextOptions<RecipeFinderDbContext> dbOptions { get; set; }

            private AppConfiguration settings { get; set; }
        }

        public static OptionsBuild ops = new OptionsBuild();

        public RecipeFinderDbContext(DbContextOptions<RecipeFinderDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Table Names
            builder.Entity<IngredientCategoryRelation>().ToTable("CategoryIngredient");
            builder.Entity<RecipeCategoryRelation>().ToTable("CategoryRecipe");

            builder.Entity<IngredientCategory>().ToTable("IngredientCategories");
            builder.Entity<RecipeCategory>().ToTable("RecipeCategories");

            builder.Entity<IngredientUnitTypeRelation>().ToTable("UnitTypeIngredient");

            builder.Entity<UserRoleRelation>().ToTable("UserRoles");

            builder.Entity<KitchenIngredient>().ToTable("Kitchens");
            builder.Entity<RequirementsListIngredient>().ToTable("IngredientLists");

            // Relations
            builder.Entity<Ingredient>()
                .HasMany(i => i.Categories)
                .WithMany(c => c.Ingredients)
                .UsingEntity<IngredientCategoryRelation>(x => x.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.CountId), x => x.HasOne(x => x.Ingredient).WithMany().HasForeignKey(x => x.IngredientId).HasPrincipalKey(x => x.Id))
                .HasKey(x => x.CountId);
            builder.Entity<IngredientCategory>().HasKey(x => x.CountId);
            builder.Entity<IngredientCategory>().Property(x => x.CountId).HasColumnName("Id");

            builder.Entity<Ingredient>()
                .HasMany(i => i.UnitTypes)
                .WithMany(i => i.Ingredients)
                .UsingEntity<IngredientUnitTypeRelation>(x => x.HasOne(x => x.UnitType).WithMany().HasForeignKey(x => x.UnitTypeId).HasPrincipalKey(x => x.CountId), x => x.HasOne(x => x.Ingredient).WithMany().HasForeignKey(x => x.IngredientId).HasPrincipalKey(x => x.Id))
                .HasKey(x => x.CountId);
            builder.Entity<UnitType>().HasKey(x => x.CountId);
            builder.Entity<UnitType>().Property(x => x.CountId).HasColumnName("Id");

            builder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<UserRoleRelation>(x => x.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).HasPrincipalKey(x => x.Id), x => x.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).HasPrincipalKey(x => x.Id))
                .HasKey(x => x.CountId);

            builder.Entity<KitchenIngredient>().HasKey(x => x.CountId);
            builder.Entity<KitchenIngredient>()
                .HasOne(x => x.Ingredient)
                .WithMany()
                .HasForeignKey(x => x.IngredientId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<KitchenIngredient>()
                .HasOne(x => x.UnitType)
                .WithMany()
                .HasForeignKey(x => x.UnitTypeId)
                .HasPrincipalKey(x => x.CountId);

            builder.Entity<KitchenIngredient>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<Kitchen>().HasKey(x => x.UserId);
            builder.Entity<Kitchen>()
                .HasOne(x => x.User)
                .WithOne(x => x.Kitchen)
                .HasForeignKey(typeof(Kitchen), nameof(Kitchen.UserId))
                .HasPrincipalKey(typeof(User), nameof(User.Id));

            builder.Entity<RequirementsList>().HasKey(x => x.RecipeId);
            builder.Entity<RequirementsList>()
                .HasOne(x => x.Recipe)
                .WithOne(x => x.RequirementsList)
                .HasForeignKey(typeof(RequirementsList), nameof(RequirementsList.RecipeId))
                .HasPrincipalKey(typeof(Recipe), nameof(Recipe.Id));

            builder.Entity<Recipe>()
                .HasMany(r => r.Categories)
                .WithMany(c => c.Recipes)
                .UsingEntity<RecipeCategoryRelation>(x => x.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.CountId), x => x.HasOne(x => x.Recipe).WithMany().HasForeignKey(x => x.RecipeId).HasPrincipalKey(x => x.Id))
                .HasKey(x => x.CountId);
            builder.Entity<RecipeCategory>().HasKey(x => x.CountId);
            builder.Entity<RecipeCategory>().Property(x => x.CountId).HasColumnName("Id");

            builder.Entity<RequirementsListIngredient>().HasKey(x => x.CountId);
            builder.Entity<RequirementsListIngredient>()
                .HasOne(x => x.Ingredient)
                .WithMany()
                .HasForeignKey(x => x.IngredientId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<RequirementsListIngredient>()
                .HasOne(x => x.UnitType)
                .WithMany()
                .HasForeignKey(x => x.UnitTypeId)
                .HasPrincipalKey(x => x.CountId);

            builder.Entity<RequirementsListIngredient>()
                .HasOne(x => x.Recipe)
                .WithMany()
                .HasForeignKey(x => x.RecipeId)
                .HasPrincipalKey(x => x.Id);

            //Column Names
            builder.Entity<IngredientCategoryRelation>().HasKey(x => x.CountId);
            builder.Entity<IngredientCategoryRelation>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<IngredientCategoryRelation>()
                .Property(x => x.CategoryId).HasColumnName("Category_id");
            builder.Entity<IngredientCategoryRelation>()
                .Property(x => x.IngredientId).HasColumnName("Ingredient_id");

            builder.Entity<RecipeCategoryRelation>().HasKey(x => x.CountId);
            builder.Entity<RecipeCategoryRelation>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<RecipeCategoryRelation>()
                .Property(x => x.CategoryId).HasColumnName("Category_id");
            builder.Entity<RecipeCategoryRelation>()
                .Property(x => x.RecipeId).HasColumnName("Recipe_id");

            builder.Entity<UnitType>().HasKey(x => x.CountId);
            builder.Entity<UnitType>()
                .Property(x => x.AllowDecimals).HasColumnName("Allow_decimals");

            builder.Entity<IngredientUnitTypeRelation>().HasKey(x => x.CountId);
            builder.Entity<IngredientUnitTypeRelation>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<IngredientUnitTypeRelation>()
                .Property(x => x.UnitTypeId).HasColumnName("Unit_type_id");
            builder.Entity<IngredientUnitTypeRelation>()
                .Property(x => x.IngredientId).HasColumnName("Ingredient_id");

            builder.Entity<Ingredient>().HasKey(x => new { x.CountId, x.Id });
            builder.Entity<Ingredient>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<Ingredient>()
                .Property(x => x.ImageLocation).HasColumnName("Image_location");
            builder.Entity<Ingredient>()
                .Property(x => x.AverageWeightInKgPerUnit).HasColumnName("Average_weight_in_kg_per_unit");
            builder.Entity<Ingredient>()
                .Property(x => x.AverageVolumeInLiterPerUnit).HasColumnName("Average_volume_in_liter_per_unit");

            builder.Entity<Recipe>().HasKey(x => new { x.CountId, x.Id });
            builder.Entity<Recipe>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<Recipe>()
                .Property(x => x.PreparationSteps).HasColumnName("Preparation_steps");
            builder.Entity<Recipe>()
                .Property(x => x.VideoTutorialLink).HasColumnName("Video_tutorial_link");
            builder.Entity<Recipe>()
                .Property(x => x.UserId).HasColumnName("Added_by_id");

            builder.Entity<KitchenIngredient>().HasKey(x => x.CountId);
            builder.Entity<KitchenIngredient>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<KitchenIngredient>()
                .Property(x => x.UserId).HasColumnName("Owner_id");
            builder.Entity<KitchenIngredient>()
                .Property(x => x.IngredientId).HasColumnName("Ingredient_id");
            builder.Entity<KitchenIngredient>()
                .Property(x => x.UnitTypeId).HasColumnName("Unit_type_id");

            builder.Entity<RequirementsListIngredient>().HasKey(x => x.CountId);
            builder.Entity<RequirementsListIngredient>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<RequirementsListIngredient>()
                .Property(x => x.RecipeId).HasColumnName("Recipe_id");
            builder.Entity<RequirementsListIngredient>()
                .Property(x => x.IngredientId).HasColumnName("Ingredient_id");
            builder.Entity<RequirementsListIngredient>()
                .Property(x => x.UnitTypeId).HasColumnName("Unit_type_id");

            builder.Entity<User>().HasKey(x => new { x.CountId, x.Id });
            builder.Entity<User>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<User>()
                .Property(x => x.PasswordHashed).HasColumnName("Password_hashed");
            builder.Entity<User>()
                .Property(x => x.SecurityStamp).HasColumnName("Security_stamp");
            builder.Entity<User>()
                .Property(x => x.ConcurrencyStamp).HasColumnName("Concurrency_stamp");
            builder.Entity<User>()
                .Property(x => x.CreationDate).HasColumnName("Creation_date");
            builder.Entity<User>()
                .Property(x => x.DOB).HasColumnName("Date_of_birth");
            builder.Entity<User>()
                .Property(x => x.EmailConfirmed).HasColumnName("Email_confirmed");
            builder.Entity<User>()
                .Property(x => x.EmailConfirmationToken).HasColumnName("Email_confirmation_token");
            builder.Entity<User>()
                .Property(x => x.PhoneNumber).HasColumnName("Phone_number");
            builder.Entity<User>()
                .Property(x => x.PhoneNumberConfirmed).HasColumnName("Phone_number_confirmed");
            builder.Entity<User>()
                .Property(x => x.LockoutEnabled).HasColumnName("lockout_enabled");
            builder.Entity<User>()
                .Property(x => x.LockoutEnd).HasColumnName("Lockout_end");
            builder.Entity<User>()
                .Property(x => x.AccessFailedCount).HasColumnName("Access_failed_count");

            builder.Entity<UserRoleRelation>().HasKey(x => x.CountId);
            builder.Entity<UserRoleRelation>()
                .Property(x => x.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<UserRoleRelation>()
                .Property(x => x.UserId).HasColumnName("User_id");
            builder.Entity<UserRoleRelation>()
                .Property(x => x.RoleId).HasColumnName("Role_id");

            builder.Entity<Role>().HasKey(x => new { x.CountId, x.Id });
            builder.Entity<Role>()
                .Property(x => x.CountId).HasColumnName("Count_id");
            //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            builder.Entity<GroceryList>().HasKey(x => new { x.CountId, x.Id });
            builder.Entity<GroceryList>()
                .Property(g => g.CountId).HasColumnName("Count_id");
                //.Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Entity<GroceryList>()
                .Property(g => g.UserId).HasColumnName("User_id");

            builder.Entity<UserAction>()
                .HasKey(a => a.CountId);
            builder.Entity<UserAction>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId).HasPrincipalKey(nameof(User.CountId));

            builder.Entity<UserAction>()
                .Property(a => a.CountId).HasColumnName("Id");
            builder.Entity<UserAction>()
                .Property(a => a.RefObjectId).HasColumnName("ref_object_id");
            builder.Entity<UserAction>()
                .Property(a => a.RefObjectName).HasColumnName("ref_object_name");
            builder.Entity<UserAction>()
                .Property(a => a.ActionPerformedOnTable).HasColumnName("Action_performed_on_table");
            builder.Entity<UserAction>()
                .Property(a => a.RequestType).HasColumnName("Request_type");
            builder.Entity<UserAction>()
                .Property(a => a.UserId).HasColumnName("User_id");
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<IngredientCategory> IngredientCategories { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }

        public DbSet<IngredientCategoryRelation> CategoriesIngredient { get; set; }
        public DbSet<RecipeCategoryRelation> CategoriesRecipe { get; set; }

        public DbSet<KitchenIngredient> Kitchens { get; set; }
        public DbSet<RequirementsListIngredient> RequirementsLists { get; set; }

        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<IngredientUnitTypeRelation> UnitTypesIngredient { get; set; }

        public DbSet<GroceryList> GroceryLists { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleRelation> UserRoles { get; set; }

        public DbSet<UserAction> UserActions { get; set; }
    }
}
