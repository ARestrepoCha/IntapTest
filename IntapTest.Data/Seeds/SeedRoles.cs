using FluentResults;
using IntapTest.Data.Entities;
using IntapTest.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntapTest.Data.Seeds
{
    public static class SeedRoles
    {
        public static async Task CreateApplicationRoles(IServiceProvider serviceProvider)
        {
			try
			{
                var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
                IdentityResult result;

                var isAdminRoleExist = await roleManager.RoleExistsAsync(nameof(RoleEnum.Administrator));
                if (!isAdminRoleExist)
                {
                    var roleAdmin = new Role
                    {
                        Name = nameof(RoleEnum.Administrator)
                    };
                    result = await roleManager.CreateAsync(roleAdmin);
                }

                var isEmployeeRoleExist = await roleManager.RoleExistsAsync(nameof(RoleEnum.Employee));
                if (!isEmployeeRoleExist)
                {
                    var roleEmployee = new Role
                    {
                        Name = nameof(RoleEnum.Employee)
                    };
                    result = await roleManager.CreateAsync(roleEmployee);
                }
            }
			catch (Exception ex)
			{
                var str = ex.Message;
            }
        }
    }
}
