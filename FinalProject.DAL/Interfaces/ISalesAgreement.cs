using FinalProject.BO.Models;

namespace FinalProject.DAL.Interfaces
{
    public interface ISalesAgreement : ICrud<SalesAgreement>
    {
        // Menambahkan satu detail ke sales agreement.
        Task AddDetailAsync(int agreementId, SalesAgreementDetail detail);

        // Menghapus satu detail dari sales agreement berdasarkan detailId.
        Task RemoveDetailAsync(int agreementId, int detailId);

        // Memperbarui satu detail pada sales agreement.
        Task UpdateDetailAsync(int agreementId, SalesAgreementDetail detail);

        // Menambahkan beberapa detail ke sales agreement secara bulk.
        Task AddDetailsAsync(int agreementId, IEnumerable<SalesAgreementDetail> details);

        // Menghapus beberapa detail dari sales agreement berdasarkan daftar detailId.
        Task RemoveDetailsAsync(int agreementId, IEnumerable<int> detailIds);

        // Memperbarui beberapa detail pada sales agreement secara bulk.
        Task UpdateDetailsAsync(int agreementId, IEnumerable<SalesAgreementDetail> details);

        // Metode non-bulk untuk entity non-detail (satu per satu)
        Task AddWithDetailsAsync(SalesAgreement agreement, SalesAgreementDetail details);
        Task UpdateWithDetailsAsync(SalesAgreement agreement, SalesAgreementDetail details);
        Task DeleteWithDetailsAsync(int agreementId);
    }
}