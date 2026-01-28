using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class MeetingRepository : Repository<Meeting>, IMeetingRepository
    {
        public MeetingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Meeting>> GetMeetingsWithDetailsAsync()
        {
            return await _context.Meeting
                .Include(m => m.Point)
                .Include(m => m.MeetingStatus)
                .Include(m => m.MeetingTopic)
                .Include(m => m.MeetingAttends)
                    .ThenInclude(ma => ma.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Meeting?> GetMeetingWithAttendeesAsync(int id)
        {
            return await _context.Meeting
                .Include(m => m.Point)
                .Include(m => m.MeetingStatus)
                .Include(m => m.MeetingTopic)
                .Include(m => m.MeetingAttends)
                    .ThenInclude(ma => ma.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsByPointAsync(int pointId)
        {
            return await _context.Meeting
                .Where(m => m.PointId == pointId)
                .Include(m => m.Point)
                .Include(m => m.MeetingStatus)
                .Include(m => m.MeetingTopic)
                .Include(m => m.MeetingAttends)
                    .ThenInclude(ma => ma.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsByDateAsync(DateOnly date)
        {
            return await _context.Meeting
                .Where(m => m.Date == date)
                .Include(m => m.Point)
                .Include(m => m.MeetingStatus)
                .Include(m => m.MeetingTopic)
                .Include(m => m.MeetingAttends)
                    .ThenInclude(ma => ma.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsByStatusAsync(int statusId)
        {
            return await _context.Meeting
                .Where(m => m.StatusId == statusId)
                .Include(m => m.Point)
                .Include(m => m.MeetingStatus)
                .Include(m => m.MeetingTopic)
                .Include(m => m.MeetingAttends)
                    .ThenInclude(ma => ma.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AddAttendeeToMeetingAsync(int meetingId, int employeeId)
        {
            var meeting = await _context.Meeting.FindAsync(meetingId);
            var employee = await _context.Employee.FindAsync(employeeId);

            if (meeting == null || employee == null)
                return false;

            var existingAttendee = await _context.MeetingAttend
                .FirstOrDefaultAsync(ma => ma.MeetingId == meetingId && ma.EmployeeId == employeeId);

            if (existingAttendee != null)
                return false;

            var meetingAttend = new MeetingAttend
            {
                MeetingId = meetingId,
                EmployeeId = employeeId
            };

            _context.MeetingAttend.Add(meetingAttend);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAttendeeFromMeetingAsync(int meetingId, int employeeId)
        {
            var meetingAttend = await _context.MeetingAttend
                .FirstOrDefaultAsync(ma => ma.MeetingId == meetingId && ma.EmployeeId == employeeId);

            if (meetingAttend == null)
                return false;

            _context.MeetingAttend.Remove(meetingAttend);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
