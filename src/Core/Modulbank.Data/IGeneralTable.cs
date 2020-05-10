using System;
using System.Threading.Tasks;

namespace Modulbank.Data
{
    public interface IGeneralTable<TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        
        Task<TEntity> UpdateAsync(TEntity entity);
        
        Task<TEntity> GetAsync(Guid userId);
    }
}