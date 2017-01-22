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
            var om = new IdentityRole { Name = RoleType.OM.ToString() };
            var th = new IdentityRole { Name = RoleType.TH.ToString() };
            var bd = new IdentityRole { Name = RoleType.BD.ToString() };
            var ah = new IdentityRole { Name = RoleType.AH.ToString() };
            var ts = new IdentityRole { Name = RoleType.TS.ToString() };

            context.Roles.AddOrUpdate(
                r => r.Name,
                om,
                th,
                bd,
                ah,
                ts
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
                    userManager.AddToRole(admin.Id, RoleType.OM.ToString());
                }
            }
            #endregion
        }
    }
}

