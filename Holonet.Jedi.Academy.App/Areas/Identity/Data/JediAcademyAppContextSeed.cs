using Holonet.Jedi.Academy.Entities.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Holonet.Jedi.Academy.Entities.Configuration.IdentityPlatform;
using Holonet.Jedi.Academy.App.Areas.Identity.Data;

namespace Holonet.Jedi.Academy.App.Areas.Identity.Data
{
    public static class JediAcademyAppContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<JediAcademyAppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Instructor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Student.ToString()));
        }

        public static async Task SeedAdminAsync(UserManager<JediAcademyAppUser> userManager, RoleManager<IdentityRole> roleManager, UserSeedInformation identitySeeds)
        {
            //Seed Default User
            var defaultUser = new JediAcademyAppUser
            {
                UserName = identitySeeds.UserName,
                Email = identitySeeds.Email,
                FirstName = identitySeeds.FirstName,
                LastName = identitySeeds.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    try
                    {
                        await userManager.CreateAsync(defaultUser, identitySeeds.DefaultPassword);
                        await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Student.ToString());
                        await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Instructor.ToString());
                        await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Administrator.ToString());
                    }
                    catch(Exception ex)
                    {
                        var error = ex.Message;
                    }
                }

            }
        }
    }
}
