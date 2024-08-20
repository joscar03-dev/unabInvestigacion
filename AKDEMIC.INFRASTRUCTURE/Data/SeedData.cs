using AKDEMIC.DOMAIN.Entities.General;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.INFRASTRUCTURE.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AkdemicContext(
                serviceProvider.GetRequiredService<DbContextOptions<AkdemicContext>>()))
            {
                // Asegurarse de que la base de datos esté creada
                context.Database.EnsureCreated();

                // Comprobar si ya existe un usuario admin
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Crear el rol admin si no existe
                if (!await roleManager.RoleExistsAsync("Administrator"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Administrator"));
                }

                // Crear el usuario admin si no existe
                var adminUser = await userManager.FindByNameAsync("admin");

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        Name = "AdminName",
                        MaternalSurname = "AdminMaternalSurname",
                        PaternalSurname = "AdminPaternalSurname",
                        FullName = "AdminName AdminPaternalSurname AdminMaternalSurname",
                        Sex = 1,
                        Type = 1,
                        BirthDate = new DateTime(1980, 1, 1),
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin123!");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Superadmin");
                    }
                }
            }
        }
    }
}
