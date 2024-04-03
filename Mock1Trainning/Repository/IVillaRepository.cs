using Mock1Trainning.Models;
using System.Linq.Expressions;

namespace Mock1Trainning.Repository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null);
        Task<Villa> Get(Expression<Func<Villa,bool>> filter = null, bool tracked = true);
        Task Create(Villa entity);
        Task Remove(Villa entity);
        Task Update(Villa entity);
        Task Save();
    }
}
