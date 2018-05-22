using SharedKernel.BaseClasses;
using System.Threading.Tasks;

namespace SharedKernel.Interfaces
{
    public interface IRepository<T, TId> where T : Entity<TId>
    {
        Task<T> Get(TId id);

        Task SaveChanges(T root);
    }
}