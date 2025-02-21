using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceProject.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            // Retrieve all roles
            var roles = await Task.FromResult(_roleManager.Roles);
            return roles.ToList();
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            var roleModel = new IdentityRole { Name = roleName };
            // Save to database
            return await _roleManager.CreateAsync(roleModel);
        }
    }
}
