using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class TitleRepository : Repository<Title>, ITitleRepository
    {
        public TitleRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<Title>> GetTitlesWithEmployeesAsync()
        {
            return await _context.Title
                .Include(t => t.Employee)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
