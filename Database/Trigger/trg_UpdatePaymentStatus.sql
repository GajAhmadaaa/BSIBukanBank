-- Trigger ini akan mengupdate status transaksi menjadi 'Lunas' jika total pembayaran >= total amount
CREATE TRIGGER trg_UpdatePaymentStatus
ON PaymentHistory
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sa
    SET Status = 'Completed'
    FROM SalesAgreement sa
    WHERE sa.SalesAgreementID IN (
        SELECT p.SalesAgreementID
        FROM inserted p
        WHERE p.SalesAgreementID IS NOT NULL
    )
    AND sa.TotalAmount IS NOT NULL
    AND (
        SELECT SUM(PaymentAmount)
        FROM PaymentHistory
        WHERE SalesAgreementID = sa.SalesAgreementID
    ) >= sa.TotalAmount;
END
GO