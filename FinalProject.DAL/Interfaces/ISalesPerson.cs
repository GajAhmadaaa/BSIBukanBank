using FinalProject.BO.Models;

namespace FinalProject.DAL.Interfaces
{
    public interface ISalesPerson : ICrud<SalesPerson>
    {
        /// <summary>
        /// Mengambil SalesPerson berdasarkan username.
        /// Mengembalikan null jika tidak ditemukan.
        /// Melempar ArgumentNullException jika username kosong.
        /// </summary>
        Task<SalesPerson?> GetByUsernameAsync(string username);
    }
}