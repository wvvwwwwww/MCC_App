 using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class RolesRepository : Repository<Roles>, IRolesRepository
    {
        public RolesRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Roles?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}
