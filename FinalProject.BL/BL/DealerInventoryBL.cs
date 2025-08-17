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
    /// Lapisan logika bisnis untuk DealerInventory.
    /// </summary>
    public class DealerInventoryBL : IDealerInventoryBL
    {
        private readonly IDealerInventory _dealerInventoryDAL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Menginisialisasi instance baru dari kelas <see cref="DealerInventoryBL"/>.
        /// </summary>
        /// <param name="dealerInventoryDAL">Lapisan akses data untuk DealerInventory.</param>
        /// <param name="mapper">Mapper untuk mapping objek.</param>
        public DealerInventoryBL(IDealerInventory dealerInventoryDAL, IMapper mapper)
        {
            _dealerInventoryDAL = dealerInventoryDAL;
            _mapper = mapper;
        }

        /// <summary>
        /// Membuat inventaris dealer baru.
        /// </summary>
        /// <param name="dealerInventory">DTO inventaris dealer yang akan dibuat.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<DealerInventoryViewDTO> CreateAsync(DealerInventoryInsertDTO dealerInventory)
        {
            // Validasi dasar
            if (dealerInventory.Stock < 0)
            {
                // Pertimbangkan untuk melempar exception validasi khusus atau menanganinya sesuai dengan strategi penanganan error aplikasi Anda
                // Untuk saat ini, kita hanya akan kembali tanpa membuat
                // Anda mungkin ingin melempar exception di sini
                throw new ArgumentException("Stock cannot be negative");
            }

            var newDealerInventory = _mapper.Map<DealerInventory>(dealerInventory);
            await _dealerInventoryDAL.CreateAsync(newDealerInventory);
            return _mapper.Map<DealerInventoryViewDTO>(newDealerInventory);
        }

        /// <summary>
        /// Menghapus inventaris dealer berdasarkan ID-nya.
        /// </summary>
        /// <param name="id">ID inventaris dealer yang akan dihapus.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task DeleteAsync(int id)
        {
            await _dealerInventoryDAL.DeleteAsync(id);
        }

        /// <summary>
        /// Mendapatkan semua inventaris dealer.
        /// </summary>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan koleksi DTO inventaris dealer.</returns>
        public async Task<IEnumerable<DealerInventoryViewDTO>> GetAllAsync()
        {
            var dealerInventories = await _dealerInventoryDAL.GetAllAsync();
            return _mapper.Map<IEnumerable<DealerInventoryViewDTO>>(dealerInventories);
        }

        /// <summary>
        /// Mendapatkan inventaris dealer berdasarkan ID-nya.
        /// </summary>
        /// <param name="id">ID inventaris dealer yang akan diambil.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan DTO inventaris dealer, atau null jika tidak ditemukan.</returns>
        public async Task<DealerInventoryViewDTO?> GetByIdAsync(int id)
        {
            var dealerInventory = await _dealerInventoryDAL.GetByIdAsync(id);
            if (dealerInventory == null)
            {
                return null;
            }
            return _mapper.Map<DealerInventoryViewDTO>(dealerInventory);
        }

        /// <summary>
    /// Memperbarui inventaris dealer yang sudah ada.
    /// </summary>
    /// <param name="id">ID inventaris dealer yang akan diperbarui.</param>
    /// <param name="dealerInventory">DTO inventaris dealer yang diperbarui.</param>
    /// <returns>Tugas yang mewakili operasi asinkron.</returns>
    public async Task<DealerInventoryViewDTO> UpdateAsync(int id, DealerInventoryUpdateDTO dealerInventory)
    {
        // Validasi dasar
        if (dealerInventory.Stock < 0)
        {
            // Pertimbangkan untuk melempar exception validasi khusus atau menanganinya sesuai dengan strategi penanganan error aplikasi Anda
            // Untuk saat ini, kita hanya akan kembali tanpa memperbarui
            // Anda mungkin ingin melempar exception di sini
            throw new ArgumentException("Stock cannot be negative");
        }

        var existingDealerInventory = await _dealerInventoryDAL.GetByIdAsync(id);
        if (existingDealerInventory != null)
        {
            _mapper.Map(dealerInventory, existingDealerInventory);
            await _dealerInventoryDAL.UpdateAsync(existingDealerInventory);
            return _mapper.Map<DealerInventoryViewDTO>(existingDealerInventory);
        }
        return null;
    }
    
    /// <summary>
    /// Mendapatkan inventaris dealer berdasarkan dealer dan mobil.
    /// </summary>
    /// <param name="dealerId">ID dealer.</param>
    /// <param name="carId">ID mobil.</param>
    /// <returns>Tugas yang mewakili operasi asinkron, dengan DTO inventaris dealer, atau null jika tidak ditemukan.</returns>
    public async Task<DealerInventoryViewDTO?> GetByDealerAndCarAsync(int dealerId, int carId)
    {
        var dealerInventory = await _dealerInventoryDAL.GetInventoryByDealerAndCarAsync(dealerId, carId);
        if (dealerInventory == null)
        {
            return null;
        }
        return _mapper.Map<DealerInventoryViewDTO>(dealerInventory);
    }
    }
}