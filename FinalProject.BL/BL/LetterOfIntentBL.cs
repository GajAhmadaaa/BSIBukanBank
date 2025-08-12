using AutoMapper;
using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    /// <summary>
    /// Lapisan logika bisnis untuk LetterOfIntent.
    /// </summary>
    public class LetterOfIntentBL : ILetterOfIntentBL
    {
        private readonly ILetterOfIntent _letterOfIntentDAL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Menginisialisasi instance baru dari kelas <see cref="LetterOfIntentBL"/>.
        /// </summary>
        /// <param name="letterOfIntentDAL">Lapisan akses data untuk LetterOfIntent.</param>
        /// <param name="mapper">Mapper untuk mapping objek.</param>
        public LetterOfIntentBL(ILetterOfIntent letterOfIntentDAL, IMapper mapper)
        {
            _letterOfIntentDAL = letterOfIntentDAL;
            _mapper = mapper;
        }

        /// <summary>
        /// Membuat surat niat baru.
        /// </summary>
        /// <param name="letterOfIntent">DTO surat niat yang akan dibuat.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<LetterOfIntentViewDTO> CreateAsync(LetterOfIntentInsertDTO letterOfIntent)
        {
            // Validasi dasar bisa ditambahkan di sini jika diperlukan
            // Misalnya, memastikan tanggal tidak di masa depan terlalu jauh, dll.
            
            var newLetterOfIntent = _mapper.Map<LetterOfIntent>(letterOfIntent);
            await _letterOfIntentDAL.CreateAsync(newLetterOfIntent);
            return _mapper.Map<LetterOfIntentViewDTO>(newLetterOfIntent);
        }

        /// <summary>
        /// Membuat surat niat baru beserta detail-detailnya.
        /// </summary>
        /// <param name="letterOfIntentWithDetails">DTO surat niat dengan detail yang akan dibuat.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<LetterOfIntentViewDTO> CreateWithDetailsAsync(LetterOfIntentWithDetailsInsertDTO letterOfIntentWithDetails)
        {
            // Validasi dasar bisa ditambahkan di sini jika diperlukan
            
            var newLetterOfIntent = _mapper.Map<LetterOfIntent>(letterOfIntentWithDetails);
            
            // Membuat letter of intent terlebih dahulu
            await _letterOfIntentDAL.CreateAsync(newLetterOfIntent);
            
            // Membuat detail-detail jika ada
            if (letterOfIntentWithDetails.Details != null && letterOfIntentWithDetails.Details.Any())
            {
                var details = letterOfIntentWithDetails.Details.Select(d => _mapper.Map<LetterOfIntentDetail>(d)).ToList();
                await _letterOfIntentDAL.AddDetailsAsync(newLetterOfIntent.Loiid, details);
            }
            
            // Mengambil kembali letter of intent dengan detail untuk memastikan semua data sudah ter-update
            var createdLetterOfIntent = await _letterOfIntentDAL.GetByIdWithDetailsAsync(newLetterOfIntent.Loiid);
            return _mapper.Map<LetterOfIntentViewDTO>(createdLetterOfIntent);
        }

        /// <summary>
        /// Menghapus surat niat berdasarkan ID-nya.
        /// </summary>
        /// <param name="id">ID surat niat yang akan dihapus.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task DeleteAsync(int id)
        {
            await _letterOfIntentDAL.DeleteAsync(id);
        }

        /// <summary>
        /// Mendapatkan semua surat niat.
        /// </summary>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan koleksi DTO surat niat.</returns>
        public async Task<IEnumerable<LetterOfIntentViewDTO>> GetAllAsync()
        {
            var letterOfIntents = await _letterOfIntentDAL.GetAllAsync();
            return _mapper.Map<IEnumerable<LetterOfIntentViewDTO>>(letterOfIntents);
        }

        /// <summary>
        /// Mendapatkan surat niat berdasarkan ID-nya.
        /// </summary>
        /// <param name="id">ID surat niat yang akan diambil.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan DTO surat niat, atau null jika tidak ditemukan.</returns>
        public async Task<LetterOfIntentViewDTO?> GetByIdAsync(int id)
        {
            var letterOfIntent = await _letterOfIntentDAL.GetByIdAsync(id);
            if (letterOfIntent == null)
            {
                return null;
            }
            return _mapper.Map<LetterOfIntentViewDTO>(letterOfIntent);
        }

        /// <summary>
        /// Memperbarui surat niat yang sudah ada.
        /// </summary>
        /// <param name="id">ID surat niat yang akan diperbarui.</param>
        /// <param name="letterOfIntent">DTO surat niat yang diperbarui.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<LetterOfIntentViewDTO> UpdateAsync(int id, LetterOfIntentUpdateDTO letterOfIntent)
        {
            // Validasi dasar bisa ditambahkan di sini jika diperlukan
            
            var existingLetterOfIntent = await _letterOfIntentDAL.GetByIdAsync(id);
            if (existingLetterOfIntent != null)
            {
                _mapper.Map(letterOfIntent, existingLetterOfIntent);
                await _letterOfIntentDAL.UpdateAsync(existingLetterOfIntent);
                return _mapper.Map<LetterOfIntentViewDTO>(existingLetterOfIntent);
            }
            return null;
        }
    }
}