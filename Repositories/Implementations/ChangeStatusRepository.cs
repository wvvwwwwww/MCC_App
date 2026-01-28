using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class ChangeStatusRepository : Repository<ChangeStatus>, IChangeStatusRepository
    {
        public ChangeStatusRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ChangeStatus?> GetStatusByNameAsync(string statusName)
        {
            return await _context.ChangeStatus
                .FirstOrDefaultAsync(cs => cs.StatusName == statusName);
        }
    }
}
