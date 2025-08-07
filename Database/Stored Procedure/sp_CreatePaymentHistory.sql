CREATE PROCEDURE sp_CreatePaymentHistory
    @SalesAgreementID INT = NULL,
    @CreditAppID INT = NULL,
    @PaymentAmount MONEY,
    @PaymentDate DATETIME,
    @PaymentType VARCHAR(20)
AS
BEGIN
    -- Validasi: minimal salah satu ID harus diisi
    IF @SalesAgreementID IS NULL AND @CreditAppID IS NULL
    BEGIN
        RAISERROR('SalesAgreementID atau CreditAppID harus diisi.', 16, 1);
        RETURN;
    END

    -- Validasi: jika SalesAgreementID diisi, pastikan ada di tabel
    IF @SalesAgreementID IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SalesAgreement WHERE SalesAgreementID = @SalesAgreementID)
    BEGIN
        RAISERROR('SalesAgreementID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Validasi: jika CreditAppID diisi, pastikan ada di tabel
    IF @CreditAppID IS NOT NULL AND NOT EXISTS (SELECT 1 FROM CreditApplication WHERE CreditAppID = @CreditAppID)
    BEGIN
        RAISERROR('CreditAppID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Insert pembayaran baru
    INSERT INTO PaymentHistory (SalesAgreementID, CreditAppID, PaymentAmount, PaymentDate, PaymentType)
    VALUES (@SalesAgreementID, @CreditAppID, @PaymentAmount, @PaymentDate, @PaymentType);

    PRINT 'Pembayaran berhasil dicatat.';
END
GO