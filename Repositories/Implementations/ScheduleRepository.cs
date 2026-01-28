using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Schedule>> GetSchedulesWithDetailsAsync()
        {
            return await _context.Schedule
                .Include(s => s.DayOfWeek)
                .Include(s => s.Point)
                .Include(s => s.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByEmployeeAsync(int employeeId)
        {
            return await _context.Schedule
                .Where(s => s.EmployeeId == employeeId)
                .Include(s => s.DayOfWeek)
                .Include(s => s.Point)
                .Include(s => s.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByPointAsync(int pointId)
        {
            return await _context.Schedule
                .Where(s => s.PointId == pointId)
                .Include(s => s.DayOfWeek)
                .Include(s => s.Point)
                .Include(s => s.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _context.Schedule
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .Include(s => s.DayOfWeek)
                .Include(s => s.Point)
                .Include(s => s.Employee)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
