using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class EmployeeScheduleRepository : Repository<EmployeeSchedule>, IEmployeeScheduleRepository
    {
        public EmployeeScheduleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeSchedule>> GetSchedulesWithEmployeesAsync()
        {
            return await _context.EmployeeSchedule
                .Include(es => es.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeSchedule>> GetSchedulesByEmployeeAsync(int employeeId)
        {
            return await _context.EmployeeSchedule
                .Where(es => es.EmployeeId == employeeId)
                .Include(es => es.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeSchedule>> GetSchedulesByDateAsync(DateOnly date)
        {
            return await _context.EmployeeSchedule
                .Where(es => es.Date == date)
                .Include(es => es.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeSchedule>> GetSchedulesByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _context.EmployeeSchedule
                .Where(es => es.Date >= startDate && es.Date <= endDate)
                .Include(es => es.Employee)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
