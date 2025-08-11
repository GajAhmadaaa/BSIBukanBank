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
    /// Lapisan logika bisnis untuk SalesPerson.
    /// </summary>
    public class SalesPersonBL : ISalesPersonBL
    {
        private readonly ISalesPerson _salesPersonDAL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Menginisialisasi instance baru dari kelas <see cref="SalesPersonBL"/>.
        /// </summary>
        /// <param name="salesPersonDAL">Lapisan akses data untuk SalesPerson.</param>
        /// <param name="mapper">Mapper untuk mapping objek.</param>
        public SalesPersonBL(ISalesPerson salesPersonDAL, IMapper mapper)
        {
            _salesPersonDAL = salesPersonDAL;
            _mapper = mapper;
        }

        /// <summary>
        /// Membuat sales person baru.
        /// </summary>
        /// <param name="salesPerson">DTO sales person yang akan dibuat.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<SalesPersonViewDTO> CreateAsync(SalesPersonInsertDTO salesPerson)
        {
            // Validasi dasar bisa ditambahkan di sini jika diperlukan
            // Misalnya, memastikan Name tidak kosong
            
            if (string.IsNullOrWhiteSpace(salesPerson.Name))
            {
                // Pertimbangkan untuk melempar exception validasi khusus
                throw new ArgumentException("Name cannot be null or empty");
            }
            
            var newSalesPerson = _mapper.Map<SalesPerson>(salesPerson);
            await _salesPersonDAL.CreateAsync(newSalesPerson);
            return _mapper.Map<SalesPersonViewDTO>(newSalesPerson);
        }

        /// <summary>
        /// Menghapus sales person berdasarkan ID-nya.
        /// </summary>
        /// <param name="id">ID sales person yang akan dihapus.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task DeleteAsync(int id)
        {
            await _salesPersonDAL.DeleteAsync(id);
        }

        /// <summary>
        /// Mendapatkan semua sales person.
        /// </summary>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan koleksi DTO sales person.</returns>
        public async Task<IEnumerable<SalesPersonViewDTO>> GetAllAsync()
        {
            var salesPersons = await _salesPersonDAL.GetAllAsync();
            return _mapper.Map<IEnumerable<SalesPersonViewDTO>>(salesPersons);
        }

        /// <summary>
        /// Mendapatkan sales person berdasarkan ID-nya.
        /// </summary>
        /// <param name="id">ID sales person yang akan diambil.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan DTO sales person, atau null jika tidak ditemukan.</returns>
        public async Task<SalesPersonViewDTO?> GetByIdAsync(int id)
        {
            var salesPerson = await _salesPersonDAL.GetByIdAsync(id);
            if (salesPerson == null)
            {
                return null;
            }
            return _mapper.Map<SalesPersonViewDTO>(salesPerson);
        }

        /// <summary>
        /// Memperbarui sales person yang sudah ada.
        /// </summary>
        /// <param name="id">ID sales person yang akan diperbarui.</param>
        /// <param name="salesPerson">DTO sales person yang diperbarui.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<SalesPersonViewDTO> UpdateAsync(int id, SalesPersonUpdateDTO salesPerson)
        {
            // Validasi dasar bisa ditambahkan di sini jika diperlukan
            if (string.IsNullOrWhiteSpace(salesPerson.Name))
            {
                // Pertimbangkan untuk melempar exception validasi khusus
                throw new ArgumentException("Name cannot be null or empty");
            }
            
            var existingSalesPerson = await _salesPersonDAL.GetByIdAsync(id);
            if (existingSalesPerson != null)
            {
                _mapper.Map(salesPerson, existingSalesPerson);
                await _salesPersonDAL.UpdateAsync(existingSalesPerson);
                return _mapper.Map<SalesPersonViewDTO>(existingSalesPerson);
            }
            return null;
        }
    }
}