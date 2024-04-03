using Microsoft.EntityFrameworkCore;
using Mock1Trainning.Data;
using Mock1Trainning.Models;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Mock1Trainning.Repository
{
    public class VillaRepository : IVillaRepository

    {
        private readonly ApplicationDbContext _context;

        public VillaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(Villa entity)
        {
            await _context.Villas.AddAsync(entity);
            await Save();

        }

        public async Task<Villa> Get(Expression<Func<Villa,bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _context.Villas;
            if(!tracked)
            {
                query = query.AsNoTracking();

            }
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null)
        {
            IQueryable<Villa> query = _context.Villas;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task Remove(Villa entity)
        {
            _context.Villas.Remove(entity);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Villa entity)
        {
            _context.Villas.Update(entity);
            await Save();
        }
    }
}
