using MccApi.Models;

namespace MccApi.Repositories.Interfaces
{
    public interface IAutorizationRepository : IRepository<Autorization>
    {
        Task<Autorization?> GetByLoginAsync(string login);
        Task<bool> ValidateCredentialsAsync(string login, string password);
        Task<Autorization?> GetByEmployeeIdAsync(int employeeId);
        Task<Autorization?> GetByLoginWithDetailsAsync(string login);
        Task<IEnumerable<Autorization>> GetByRoleAsync(int roleId);
    }
}
