using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class ChangesHistoryRepository : Repository<ChangesHistory>, IChangesHistoryRepository
    {
        public ChangesHistoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ChangesHistory>> GetHistoryWithDetailsAsync()
        {
            return await _context.ChangesHistory
                .Include(ch => ch.Schedule)
                .Include(ch => ch.Employee)
                .Include(ch => ch.Point)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ChangesHistory>> GetHistoryByScheduleAsync(int scheduleId)
        {
            return await _context.ChangesHistory
                .Where(ch => ch.ScheduleId == scheduleId)
                .Include(ch => ch.Schedule)
                .Include(ch => ch.Employee)
                .Include(ch => ch.Point)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ChangesHistory>> GetHistoryByEmployeeAsync(int employeeId)
        {
            return await _context.ChangesHistory
                .Where(ch => ch.EmployeeId == employeeId)
                .Include(ch => ch.Schedule)
                .Include(ch => ch.Employee)
                .Include(ch => ch.Point)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ChangesHistory>> GetHistoryByPointAsync(int pointId)
        {
            return await _context.ChangesHistory
                .Where(ch => ch.PointId == pointId)
                .Include(ch => ch.Schedule)
                .Include(ch => ch.Employee)
                .Include(ch => ch.Point)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
