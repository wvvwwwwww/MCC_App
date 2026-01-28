using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class PointRepository : Repository<Point>, IPointRepository
    {
        public PointRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Point>> GetPointsWithEmployeesAsync()
        {
            return await _context.Point
                .Include(p => p.GeneralEmployee)
                .AsNoTracking()
                .ToListAsync();
        }
     public async Task<IEnumerable<Point>> GetPointsWithKitchenAsync()
        {
            return await _context.Point
                .Where(p => p.Kitchen)
                .Include(p => p.GeneralEmployee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Point>> GetPointsByEmployeeAsync(int employeeId)
        {
            return await _context.Point
                .Where(p => p.GeneralEmployeeId == employeeId)
                .Include(p => p.GeneralEmployee)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
