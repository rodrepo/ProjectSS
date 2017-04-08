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

        protected override void Seed(DataContext context)
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

            var demoOM = userManager.FindByEmail("om@demo.com");
            if (demoOM == null)
            {
                demoOM = new User
                {
                    FirstName = "Rodolfo",
                    LastName = "Romarate",
                    UserName = "om@demo.com",
                    Email = "om@demo.com",
                    Rate = 2000,
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = userManager.Create(demoOM, password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(demoOM.Id, RoleType.OM.ToString());
                }
            }
            var demoBD = userManager.FindByEmail("bd@demo.com");
            if (demoBD == null)
            {
                demoBD = new User
                {
                    FirstName = "BD",
                    LastName = "Demo",
                    UserName = "bd@demo.com",
                    Email = "bd@demo.com",
                    Rate = 1500,
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = userManager.Create(demoBD, password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(demoBD.Id, RoleType.BD.ToString());
                }
            }
            var demoTH = userManager.FindByEmail("th@demo.com");
            if (demoTH == null)
            {
                demoTH = new User
                {
                    FirstName = "TH",
                    LastName = "Demo",
                    UserName = "th@demo.com",
                    Email = "th@demo.com",
                    Rate = 1000,
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = userManager.Create(demoTH, password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(demoTH.Id, RoleType.TH.ToString());
                }
            }
            var demoAH = userManager.FindByEmail("ah@demo.com");
            if (demoAH == null)
            {
                demoAH = new User
                {
                    FirstName = "AH",
                    LastName = "Demo",
                    UserName = "ah@demo.com",
                    Email = "ah@demo.com",
                    Rate = 800,
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = userManager.Create(demoAH, password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(demoAH.Id, RoleType.AH.ToString());
                }
            }
            var demoTS = userManager.FindByEmail("ts@demo.com");
            if (demoTS == null)
            {
                demoTS = new User
                {
                    FirstName = "TS",
                    LastName = "Demo",
                    UserName = "ts@demo.com",
                    Email = "ts@demo.com",
                    Rate = 500,
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = userManager.Create(demoTS, password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(demoTS.Id, RoleType.TS.ToString());
                }
            }
            context.SaveChanges();
            #endregion
        }
    }
}

