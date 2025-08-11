using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FinalProject.DAL.DAL
{
    public class BaseDAL<T> : ICrud<T> where T : class
    {
        protected readonly FinalProjectContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseDAL(FinalProjectContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> CreateAsync(T item)
        {
            try
            {
                await _dbSet.AddAsync(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal menambah data: {ex.Message}", ex);
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal mengambil data berdasarkan ID: {ex.Message}", ex);
            }
        }

        public async Task<T> UpdateAsync(T item)
        {
            try
            {
                _dbSet.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal mengupdate data: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal menghapus data: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal mengambil data: {ex.Message}", ex);
            }
        }
    }
}