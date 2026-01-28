using MccApi.Data;
using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MccApi.Repositories.Implementations
{
    public class MeetingTopicRepository : Repository<MeetingTopic>, IMeetingTopicRepository
    {
        public MeetingTopicRepository(ApplicationDbContext context) : base(context) { }

        public async Task<MeetingTopic?> GetTopicByNameAsync(string topicName)
        {
            return await _context.MeetingTopic
                .FirstOrDefaultAsync(mt => mt.TopicName == topicName);
        }
    }
}
