namespace ProjectSS.Db.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ProjectSS.Common;
    using ProjectSS.Db.Entities;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjectSS.Db.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProjectSS.Db.DataContext context)
        {
            #region Seed Roles
            var om = new IdentityRole { Name = RoleType.Om.ToString() };
            var staff = new IdentityRole { Name = RoleType.Staff.ToString() };

            context.Roles.AddOrUpdate(
                r => r.Name,
                om,
                staff
                );
            context.SaveChanges();
            #endregion

            #region Seed Users
            var userManager = new UserManager<User>(new UserStore<User>(context));
            userManager.UserValidator = new UserValidator<User>(userManager) { AllowOnlyAlphanumericUserNames = false };
            const string password = "P@ssw0rd";

            var admin = userManager.FindByEmail("om@demo.com");
            if (admin == null)
            {
                admin = new User
                {
                    FirstName = "OM",
                    LastName = "Demo",
                    UserName = "om@demo.com",
                    Email = "om@demo.com",
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = userManager.Create(admin, password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(admin.Id, RoleType.Om.ToString());
                }
            }
            #endregion
        }
    }
}

