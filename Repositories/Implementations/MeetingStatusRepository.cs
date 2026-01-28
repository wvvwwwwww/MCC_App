using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class MeetingStatusRepository : Repository<MeetingStatus>, IMeetingStatusRepository
    {
        public MeetingStatusRepository(ApplicationDbContext context) : base(context) { }

        public async Task<MeetingStatus?> GetStatusByNameAsync(string statusName)
        {
            return await _context.MeetingStatuses
                .FirstOrDefaultAsync(ms => ms.StatusName == statusName);
        }
    }
}
