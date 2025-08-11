using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FinalProject.DAL.DAL
{
    public class SalesPersonDAL : BaseDAL<SalesPerson>, ISalesPerson
    {
        public SalesPersonDAL(FinalProjectContext context) : base(context)
        {
        }

        /// Mengambil SalesPerson berdasarkan username.
        /// Mengembalikan null jika tidak ditemukan.
        /// Melempar ArgumentNullException jika username kosong.
        public async Task<SalesPerson?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Username tidak boleh kosong.");

            // Gunakan AsNoTracking untuk query read-only agar lebih efisien
            return await _context.SalesPeople
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name == username)
                .ConfigureAwait(false);
        }
    }
}