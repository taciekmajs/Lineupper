using Lineupper.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Lineupper.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly LineupperDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(LineupperDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Remove(T entity) => _dbSet.Remove(entity);
        public void Update(T entity) => _dbSet.Update(entity); 
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
