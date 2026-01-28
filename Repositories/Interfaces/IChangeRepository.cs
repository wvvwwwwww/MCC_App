using MccApi.Models;

namespace MccApi.Repositories.Interfaces
{
    public interface IChangeRepository : IRepository<Change>
    {
        Task<IEnumerable<Change>> GetChangesWithDetailsAsync();
        Task<IEnumerable<Change>> GetChangesByEmployeeAsync(int employeeId);
        Task<IEnumerable<Change>> GetChangesByStatusAsync(int statusId);
        Task<IEnumerable<Change>> GetChangesByDateRangeAsync(DateOnly startDate, DateOnly endDate);
    }
}
