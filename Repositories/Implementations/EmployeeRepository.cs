using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Employee>> GetEmployeesWithTitlesAsync()
        {
            return await _context.Employee
                .Include(e => e.Title)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
        {
            return await _context.Employee
                .Include(e => e.Title)
                .Include(e => e.Schedules)
                .Include(e => e.Changes)
                .Include(e => e.EmployeeSchedules)
                .Include(e => e.MeetingAttends)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByTitleAsync(int titleId)
        {
            return await _context.Employee
                .Where(e => e.TitleId == titleId)
                .Include(e => e.Title)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            return await _context.Employee
                .Where(e => e.Name.Contains(searchTerm) ||
                           (e.SecondName != null && e.SecondName.Contains(searchTerm)) ||
                           e.Number.Contains(searchTerm))
                .Include(e => e.Title)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
