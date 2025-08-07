CREATE PROCEDURE sp_CreateWarrantyClaim
    @CustomerID INT,
    @SalesAgreementDetailID INT,
    @ClaimDate DATETIME,
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

    -- Validasi: pastikan SalesAgreementDetailID ada di tabel
    IF NOT EXISTS (SELECT 1 FROM SalesAgreementDetail WHERE SalesAgreementDetailID = @SalesAgreementDetailID)
    BEGIN
        RAISERROR('SalesAgreementDetailID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Insert klaim garansi baru
    INSERT INTO WarrantyClaim (CustomerID, SalesAgreementDetailID, ClaimDate, Description, Status)
    VALUES (@CustomerID, @SalesAgreementDetailID, @ClaimDate, @Description, @Status);

    PRINT 'Klaim garansi berhasil dicatat.';
END
GO