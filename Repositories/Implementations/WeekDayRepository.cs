using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class WeekDayRepository : Repository<WeekDay>, IWeekDayRepository // ← Изменили здесь
    {
        public WeekDayRepository(ApplicationDbContext context) : base(context) { }

        public async Task<WeekDay?> GetDayByNameAsync(string dayName) // ← Изменили здесь
        {
            return await _context.DaysOfWeek
                .FirstOrDefaultAsync(d => d.DayOfWeekName == dayName);
        }

        public async Task<IEnumerable<WeekDay>> GetInventoryDaysAsync() // ← Изменили здесь
        {
            return await _context.DaysOfWeek
                .Where(d => d.IsDayOfInventory == true)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

