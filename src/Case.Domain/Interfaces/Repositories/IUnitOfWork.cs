using System.Threading.Tasks;

namespace Case.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}