using AutoMapper;
using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    /// <summary>
    /// Lapisan logika bisnis untuk SalesAgreement.
    /// </summary>
    public class SalesAgreementBL : ISalesAgreementBL
    {
        private readonly ISalesAgreement _salesAgreementDAL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Menginisialisasi instance baru dari kelas <see cref="SalesAgreementBL"/>.
        /// </summary>
        /// <param name="salesAgreementDAL">Lapisan akses data untuk SalesAgreement.</param>
        /// <param name="mapper">Mapper untuk mapping objek.</param>
        public SalesAgreementBL(ISalesAgreement salesAgreementDAL, IMapper mapper)
        {
            _salesAgreementDAL = salesAgreementDAL;
            _mapper = mapper;
        }

        /// <summary>
        /// Membuat perjanjian penjualan baru.
        /// </summary>
        /// <param name="salesAgreement">DTO perjanjian penjualan yang akan dibuat.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<SalesAgreementViewDTO> CreateAsync(SalesAgreementInsertDTO salesAgreement)
        {
            // Validasi dasar bisa ditambahkan di sini jika diperlukan
            // Misalnya, memastikan TotalAmount positif, Status valid, dll.
            
            var newSalesAgreement = _mapper.Map<SalesAgreement>(salesAgreement);
            await _salesAgreementDAL.CreateAsync(newSalesAgreement);
            return _mapper.Map<SalesAgreementViewDTO>(newSalesAgreement);
        }

        /// <summary>
        /// Menghapus perjanjian penjualan berdasarkan ID-nya.
        /// </summary>
        /// <param name="id">ID perjanjian penjualan yang akan dihapus.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task DeleteAsync(int id)
        {
            await _salesAgreementDAL.DeleteAsync(id);
        }

        /// <summary>
        /// Mendapatkan semua perjanjian penjualan.
        /// </summary>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan koleksi DTO perjanjian penjualan.</returns>
        public async Task<IEnumerable<SalesAgreementViewDTO>> GetAllAsync()
        {
            var salesAgreements = await _salesAgreementDAL.GetAllAsync();
            return _mapper.Map<IEnumerable<SalesAgreementViewDTO>>(salesAgreements);
        }

        /// <summary>
        /// Mendapatkan perjanjian penjualan berdasarkan ID-nya.
        /// </summary>
        /// <param name="id">ID perjanjian penjualan yang akan diambil.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan DTO perjanjian penjualan, atau null jika tidak ditemukan.</returns>
        public async Task<SalesAgreementViewDTO?> GetByIdAsync(int id)
        {
            var salesAgreement = await _salesAgreementDAL.GetByIdAsync(id);
            if (salesAgreement == null)
            {
                return null;
            }
            return _mapper.Map<SalesAgreementViewDTO>(salesAgreement);
        }

        /// <summary>
        /// Memperbarui perjanjian penjualan yang sudah ada.
        /// </summary>
        /// <param name="id">ID perjanjian penjualan yang akan diperbarui.</param>
        /// <param name="salesAgreement">DTO perjanjian penjualan yang diperbarui.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<SalesAgreementViewDTO> UpdateAsync(int id, SalesAgreementUpdateDTO salesAgreement)
        {
            // Validasi dasar bisa ditambahkan di sini jika diperlukan
            
            var existingSalesAgreement = await _salesAgreementDAL.GetByIdAsync(id);
            if (existingSalesAgreement != null)
            {
                _mapper.Map(salesAgreement, existingSalesAgreement);
                await _salesAgreementDAL.UpdateAsync(existingSalesAgreement);
                return _mapper.Map<SalesAgreementViewDTO>(existingSalesAgreement);
            }
            return null;
        }
    }
}