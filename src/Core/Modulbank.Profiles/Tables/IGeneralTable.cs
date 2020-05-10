using System;
using System.Threading.Tasks;

namespace Modulbank.Users.Tables
{
    public interface IGeneralTable<TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        
        Task<TEntity> UpdateAsync(TEntity entity);
        
        Task<TEntity> GetAsync(Guid userId);
    }
}