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
        private readonly ILetterOfIntent _letterOfIntentDAL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Menginisialisasi instance baru dari kelas <see cref="SalesAgreementBL"/>.
        /// </summary>
        /// <param name="salesAgreementDAL">Lapisan akses data untuk SalesAgreement.</param>
        /// <param name="letterOfIntentDAL">Lapisan akses data untuk LetterOfIntent.</param>
        /// <param name="mapper">Mapper untuk mapping objek.</param>
        public SalesAgreementBL(ISalesAgreement salesAgreementDAL, ILetterOfIntent letterOfIntentDAL, IMapper mapper)
        {
            _salesAgreementDAL = salesAgreementDAL;
            _letterOfIntentDAL = letterOfIntentDAL;
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
        /// Membuat perjanjian penjualan baru beserta detail-detailnya.
        /// </summary>
        /// <param name="salesAgreementWithDetails">DTO perjanjian penjualan dengan detail yang akan dibuat.</param>
        /// <returns>Tugas yang mewakili operasi asinkron.</returns>
        public async Task<SalesAgreementViewDTO> CreateWithDetailsAsync(SalesAgreementWithDetailsInsertDTO salesAgreementWithDetails)
        {
            // Validasi dasar bisa ditambahkan di sini jika diperlukan
            
            var newSalesAgreement = _mapper.Map<SalesAgreement>(salesAgreementWithDetails);
            
            // Membuat sales agreement terlebih dahulu
            await _salesAgreementDAL.CreateAsync(newSalesAgreement);
            
            // Membuat detail-detail jika ada
            if (salesAgreementWithDetails.Details != null && salesAgreementWithDetails.Details.Any())
            {
                var details = salesAgreementWithDetails.Details.Select(d => _mapper.Map<SalesAgreementDetail>(d)).ToList();
                await _salesAgreementDAL.AddDetailsAsync(newSalesAgreement.SalesAgreementId, details);
            }
            
            // Mengambil kembali sales agreement dengan detail untuk memastikan semua data sudah ter-update
            var createdSalesAgreement = await _salesAgreementDAL.GetByIdWithDetailsAsync(newSalesAgreement.SalesAgreementId);
            return _mapper.Map<SalesAgreementViewDTO>(createdSalesAgreement);
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
            var salesAgreement = await _salesAgreementDAL.GetByIdWithDetailsAsync(id);
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
        
        /// <summary>
        /// Mendapatkan semua perjanjian penjualan berdasarkan ID customer.
        /// </summary>
        /// <param name="customerId">ID customer.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan koleksi DTO perjanjian penjualan.</returns>
        public async Task<IEnumerable<SalesAgreementViewDTO>> GetByCustomerIdAsync(int customerId)
        {
            var salesAgreements = await _salesAgreementDAL.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<SalesAgreementViewDTO>>(salesAgreements);
        }
        
        /// <summary>
        /// Mendapatkan perjanjian penjualan dengan status unpaid berdasarkan ID customer.
        /// </summary>
        /// <param name="customerId">ID customer.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan koleksi DTO perjanjian penjualan unpaid.</returns>
        public async Task<IEnumerable<SalesAgreementViewDTO>> GetUnpaidByCustomerIdAsync(int customerId)
        {
            var salesAgreements = await _salesAgreementDAL.GetUnpaidByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<SalesAgreementViewDTO>>(salesAgreements);
        }
        
        /// <summary>
        /// Mendapatkan perjanjian penjualan dengan status paid berdasarkan ID customer.
        /// </summary>
        /// <param name="customerId">ID customer.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan koleksi DTO perjanjian penjualan paid.</returns>
        public async Task<IEnumerable<SalesAgreementViewDTO>> GetPaidByCustomerIdAsync(int customerId)
        {
            var salesAgreements = await _salesAgreementDAL.GetPaidByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<SalesAgreementViewDTO>>(salesAgreements);
        }
        
        /// <summary>
        /// Mengkonversi LetterOfIntent menjadi SalesAgreement.
        /// </summary>
        /// <param name="loiId">ID LetterOfIntent yang akan dikonversi.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan DTO perjanjian penjualan yang dibuat.</returns>
        public async Task<SalesAgreementViewDTO> ConvertFromLOIAsync(int loiId)
        {
            // Mendapatkan LOI dengan detail
            var loi = await _letterOfIntentDAL.GetByIdWithDetailsAsync(loiId);
            if (loi == null)
            {
                throw new Exception("LetterOfIntent not found");
            }
            
            // Memastikan LOI dalam status yang benar
            if (loi.Status != "ReadyForAgreement")
            {
                throw new Exception("LetterOfIntent is not ready for agreement conversion");
            }
            
            // Membuat SalesAgreement dari LOI
            var salesAgreement = new SalesAgreement
            {
                Loiid = loi.Loiid,
                DealerId = loi.DealerId,
                CustomerId = loi.CustomerId,
                SalesPersonId = loi.SalesPersonId ?? 0, // Handle nullable int
                TransactionDate = DateTime.Now,
                TotalAmount = 0, // Akan dihitung berdasarkan detail
                Status = "Unpaid" // Status awal adalah unpaid
            };
            
            // Membuat detail SalesAgreement dari detail LOI
            var agreementDetails = new List<SalesAgreementDetail>();
            decimal totalAmount = 0;
            
            foreach (var loiDetail in loi.LetterOfIntentDetails)
            {
                var agreementDetail = new SalesAgreementDetail
                {
                    CarId = loiDetail.CarId,
                    Price = loiDetail.AgreedPrice,
                    Discount = loiDetail.Discount,
                    Note = loiDetail.Note
                };
                
                agreementDetails.Add(agreementDetail);
                // Hitung total amount (bisa menggunakan fungsi database fn_GetFinalPrice)
                totalAmount += loiDetail.AgreedPrice - (loiDetail.Discount ?? 0);
            }
            
            salesAgreement.TotalAmount = totalAmount;
            
            // Menyimpan SalesAgreement
            await _salesAgreementDAL.CreateAsync(salesAgreement);
            
            // Menyimpan detail SalesAgreement
            if (agreementDetails.Any())
            {
                await _salesAgreementDAL.AddDetailsAsync(salesAgreement.SalesAgreementId, agreementDetails);
            }
            
            // Mengubah status LOI menjadi "Converted"
            loi.Status = "Converted";
            await _letterOfIntentDAL.UpdateAsync(loi);
            
            // Mengambil kembali sales agreement dengan detail
            var createdSalesAgreement = await _salesAgreementDAL.GetByIdWithDetailsAsync(salesAgreement.SalesAgreementId);
            return _mapper.Map<SalesAgreementViewDTO>(createdSalesAgreement);
        }
        
        /// <summary>
        /// Mengubah status SalesAgreement dari unpaid ke paid.
        /// </summary>
        /// <param name="agreementId">ID SalesAgreement yang akan diubah statusnya.</param>
        /// <returns>Tugas yang mewakili operasi asinkron, dengan DTO perjanjian penjualan yang diperbarui.</returns>
        public async Task<SalesAgreementViewDTO> MarkAsPaidAsync(int agreementId)
        {
            // Mendapatkan SalesAgreement dengan detail
            var salesAgreement = await _salesAgreementDAL.GetByIdWithDetailsAsync(agreementId);
            if (salesAgreement == null)
            {
                throw new Exception("SalesAgreement not found");
            }
            
            // Memastikan SalesAgreement dalam status yang benar
            if (salesAgreement.Status != "Unpaid")
            {
                throw new Exception("SalesAgreement is not in unpaid status");
            }
            
            // Mengubah status menjadi "Paid" (bukan "Completed")
            salesAgreement.Status = "Paid";
            
            // Memperbarui SalesAgreement di database
            await _salesAgreementDAL.UpdateAsync(salesAgreement);
            
            // Mengambil kembali sales agreement dengan detail
            var updatedSalesAgreement = await _salesAgreementDAL.GetByIdWithDetailsAsync(agreementId);
            return _mapper.Map<SalesAgreementViewDTO>(updatedSalesAgreement);
        }
    }
}