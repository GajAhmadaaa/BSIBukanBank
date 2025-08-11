using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICrud<T>
{
    Task<T> CreateAsync(T item);
    Task<T?> GetByIdAsync(int id);
    Task<T> UpdateAsync(T item);
    Task DeleteAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}
