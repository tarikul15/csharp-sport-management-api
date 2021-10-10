using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Repositories
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}