using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class AutorizationRepository : Repository<Autorization>, IAutorizationRepository
    {
        public AutorizationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Autorization?> GetByLoginAsync(string login)
        {
            return await _context.Autorization
                .Include(a => a.Employee)
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Login == login);
        }

        public async Task<bool> ValidateCredentialsAsync(string login, string password)
        {
            var autorization = await _context.Autorization
                .FirstOrDefaultAsync(a => a.Login == login && a.Password == password);

            return autorization != null;
        }

        public async Task<Autorization?> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Autorization
                .Include(a => a.Employee)
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId);
        }
        public async Task<Autorization?> GetByLoginWithDetailsAsync(string login)
        {
            return await _context.Autorization
                .Include(a => a.Employee)    // Включаем данные сотрудника
                .Include(a => a.Role)        // Включаем данные роли
                .FirstOrDefaultAsync(a => a.Login == login);
        }
        public async Task<IEnumerable<Autorization>> GetByRoleAsync(int roleId)
        {
            return await _context.Autorization
                .Where(a => a.RoleId == roleId)
                .Include(a => a.Employee)
                .Include(a => a.Role)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
