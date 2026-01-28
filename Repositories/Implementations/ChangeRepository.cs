using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class ChangeRepository : Repository<Change>, IChangeRepository
    {
        public ChangeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Change>> GetChangesWithDetailsAsync()
        {
            return await _context.Change
                .Include(c => c.Employee)
                .Include(c => c.ChangeStatus)
                .Include(c => c.EmployeeSchedule)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Change>> GetChangesByEmployeeAsync(int employeeId)
        {
            return await _context.Change
                .Where(c => c.EmployeeId == employeeId)
                .Include(c => c.Employee)
                .Include(c => c.ChangeStatus)
                .Include(c => c.EmployeeSchedule)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Change>> GetChangesByStatusAsync(int statusId)
        {
            return await _context.Change
                .Where(c => c.StatusId == statusId)
                .Include(c => c.Employee)
                .Include(c => c.ChangeStatus)
                .Include(c => c.EmployeeSchedule)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Change>> GetChangesByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _context.Change
                .Include(c => c.EmployeeSchedule)
                .Where(c => c.EmployeeSchedule != null &&
                           c.EmployeeSchedule.Date >= startDate &&
                           c.EmployeeSchedule.Date <= endDate)
                .Include(c => c.Employee)
                .Include(c => c.ChangeStatus)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
