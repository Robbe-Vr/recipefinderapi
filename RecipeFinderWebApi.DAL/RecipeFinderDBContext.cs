using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RecipeFinderWebApi.Exchange.DTOs;

namespace RecipeFinderWebApi.DAL
{
    public class RecipeFinderDBContext : DbContext
    {
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                settings = new AppConfiguration();
                opsBuilder = new DbContextOptionsBuilder<RecipeFinderDBContext>();
                opsBuilder.UseSqlServer(settings.sqlConnectionString);
                dbOptions = opsBuilder.Options;
            }
            public DbContextOptionsBuilder<RecipeFinderDBContext> opsBuilder { get; set; }

            public DbContextOptions<RecipeFinderDBContext> dbOptions { get; set; }

            private AppConfiguration settings { get; set; }
        }

        public static OptionsBuild ops = new OptionsBuild();

        public RecipeFinderDBContext(DbContextOptions<RecipeFinderDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relations
            builder.Entity<Ingredient>()
                .HasMany(i => i.Categories)
                .WithMany(c => c.Ingredients)
                .UsingEntity<IngredientCategoryRelation>(x => x.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId), x => x.HasOne(x => x.Ingredient).WithMany().HasForeignKey(x => x.IngredientId))
                .HasKey(x => x.CountId);

            builder.Entity<Ingredient>()
                .HasMany(i => i.UnitTypes)
                .WithMany(i => i.Ingredients)
                .UsingEntity<IngredientUnitTypeRelation>(x => x.HasOne(x => x.UnitType).WithMany().HasForeignKey(x => x.UnitTypeId), x => x.HasOne(x => x.Ingredient).WithMany().HasForeignKey(x => x.IngredientId))
                .HasKey(x => x.CountId);

            builder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<UserRoleRelation>(x => x.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId), x => x.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId))
                .HasKey(x => x.CountId);

            builder.Entity<KitchenIngredient>().HasKey(x => x.CountId);
            builder.Entity<KitchenIngredient>()
                .HasOne(x => x.Ingredient)
                .WithMany()
                .HasForeignKey(x => x.IngredientId);

            builder.Entity<KitchenIngredient>()
                .HasOne(x => x.UnitType)
                .WithMany()
                .HasForeignKey(x => x.UnitTypeId);

            builder.Entity<KitchenIngredient>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.Entity<Kitchen>().HasKey(x => x.UserId);
            builder.Entity<Kitchen>()
                .HasOne(x => x.User)
                .WithOne(x => x.Kitchen)
                .HasForeignKey(typeof(Kitchen), nameof(Kitchen.UserId));

            builder.Entity<RequirementsList>().HasKey(x => x.RecipeId);
            builder.Entity<RequirementsList>()
                .HasOne(x => x.Recipe)
                .WithOne(x => x.RequirementsList)
                .HasForeignKey(typeof(RequirementsList), nameof(RequirementsList.RecipeId));

            builder.Entity<Recipe>()
                .HasMany(r => r.Categories)
                .WithMany(c => c.Recipes)
                .UsingEntity<RecipeCategoryRelation>(x => x.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId), x => x.HasOne(x => x.Recipe).WithMany().HasForeignKey(x => x.RecipeId))
                .HasKey(x => x.CountId);

            builder.Entity<RequirementsListIngredient>().HasKey(x => x.CountId);
            builder.Entity<RequirementsListIngredient>()
                .HasOne(x => x.Ingredient)
                .WithMany()
                .HasForeignKey(x => x.IngredientId);

            builder.Entity<RequirementsListIngredient>()
                .HasOne(x => x.UnitType)
                .WithMany()
                .HasForeignKey(x => x.UnitTypeId);

            builder.Entity<RequirementsListIngredient>()
                .HasOne(x => x.Recipe)
                .WithMany()
                .HasForeignKey(x => x.RecipeId);

            // Table Names
            builder.Entity<IngredientCategoryRelation>().ToTable("CategoryIngredient");
            builder.Entity<RecipeCategoryRelation>().ToTable("CategoryRecipe");

            builder.Entity<IngredientCategory>().ToTable("IngredientCategories");
            builder.Entity<RecipeCategory>().ToTable("RecipeCategories");

            builder.Entity<IngredientUnitTypeRelation>().ToTable("UnitTypeIngredient");

            builder.Entity<UserRoleRelation>().ToTable("UserRoles");

            builder.Entity<KitchenIngredient>().ToTable("Kitchens");
            builder.Entity<RequirementsListIngredient>().ToTable("IngredientLists");

            //Column Names
            builder.Entity<IngredientCategoryRelation>()
                .Property(x => x.CountId).HasColumnName("Count_id");
            builder.Entity<IngredientCategoryRelation>()
                .Property(x => x.CategoryId).HasColumnName("Category_id");
            builder.Entity<IngredientCategoryRelation>()
                .Property(x => x.IngredientId).HasColumnName("Ingredient_id");

            builder.Entity<RecipeCategoryRelation>()
                .Property(x => x.CountId).HasColumnName("Count_id");
            builder.Entity<RecipeCategoryRelation>()
                .Property(x => x.CategoryId).HasColumnName("Category_id");
            builder.Entity<RecipeCategoryRelation>()
                .Property(x => x.RecipeId).HasColumnName("Recipe_id");

            builder.Entity<UnitType>()
                .Property(x => x.AllowDecimals).HasColumnName("Allow_decimals");

            builder.Entity<IngredientUnitTypeRelation>()
                .Property(x => x.CountId).HasColumnName("Count_id");
            builder.Entity<IngredientUnitTypeRelation>()
                .Property(x => x.UnitTypeId).HasColumnName("Unit_type_id");
            builder.Entity<IngredientUnitTypeRelation>()
                .Property(x => x.IngredientId).HasColumnName("Ingredient_id");

            builder.Entity<Recipe>()
                .Property(x => x.PreparationSteps).HasColumnName("Preparation_steps");
            builder.Entity<Recipe>()
                .Property(x => x.VideoTutorialLink).HasColumnName("Video_tutorial_link");
            builder.Entity<Recipe>()
                .Property(x => x.UserId).HasColumnName("Added_by_id");

            builder.Entity<KitchenIngredient>()
                .Property(x => x.CountId).HasColumnName("Count_id");
            builder.Entity<KitchenIngredient>()
                .Property(x => x.UserId).HasColumnName("Owner_id");
            builder.Entity<KitchenIngredient>()
                .Property(x => x.IngredientId).HasColumnName("Ingredient_id");
            builder.Entity<KitchenIngredient>()
                .Property(x => x.UnitTypeId).HasColumnName("Unit_type_id");

            builder.Entity<RequirementsListIngredient>()
                .Property(x => x.CountId).HasColumnName("Count_id");
            builder.Entity<RequirementsListIngredient>()
                .Property(x => x.RecipeId).HasColumnName("Recipe_id");
            builder.Entity<RequirementsListIngredient>()
                .Property(x => x.IngredientId).HasColumnName("Ingredient_id");
            builder.Entity<RequirementsListIngredient>()
                .Property(x => x.UnitTypeId).HasColumnName("Unit_type_id");

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

            builder.Entity<UserRoleRelation>()
                .Property(x => x.CountId).HasColumnName("Count_id");
            builder.Entity<UserRoleRelation>()
                .Property(x => x.UserId).HasColumnName("User_id");
            builder.Entity<UserRoleRelation>()
                .Property(x => x.RoleId).HasColumnName("Role_id");

            builder.Entity<GroceryList>()
                .Property(g => g.UserId).HasColumnName("User_id");
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

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleRelation> UserRoles { get; set; }

        public DbSet<GroceryList> GroceryLists { get; set; }
    }
}
