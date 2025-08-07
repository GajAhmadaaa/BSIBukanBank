CREATE PROCEDURE sp_CreateCustomerComplaint
    @CustomerID INT,
    @SalesAgreementID INT = NULL,
    @ComplaintDate DATETIME,
    @Description VARCHAR(200),
    @Status VARCHAR(20)
AS
BEGIN
    -- Validasi: pastikan customer ada
    IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerID = @CustomerID)
    BEGIN
        RAISERROR('CustomerID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Validasi: jika SalesAgreementID diisi, pastikan ada di tabel
    IF @SalesAgreementID IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SalesAgreement WHERE SalesAgreementID = @SalesAgreementID)
    BEGIN
        RAISERROR('SalesAgreementID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Insert komplain baru
    INSERT INTO CustomerComplaint (CustomerID, SalesAgreementID, ComplaintDate, Description, Status)
    VALUES (@CustomerID, @SalesAgreementID, @ComplaintDate, @Description, @Status);

    PRINT 'Komplain pelanggan berhasil dicatat.';
END
GO