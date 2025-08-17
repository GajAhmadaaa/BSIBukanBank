CREATE PROCEDURE sp_CreateCustomerNotification
    @CustomerID INT,
    @LOIID INT = NULL,
    @SalesAgreementID INT = NULL,
    @NotificationType VARCHAR(50),
    @Message VARCHAR(500)
AS
BEGIN
    -- Validasi: pastikan customer ada
    IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerID = @CustomerID)
    BEGIN
        RAISERROR('CustomerID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Validasi: pastikan LOI ada jika diisi
    IF @LOIID IS NOT NULL AND NOT EXISTS (SELECT 1 FROM LetterOfIntent WHERE LOIID = @LOIID)
    BEGIN
        RAISERROR('LOIID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Validasi: pastikan SalesAgreement ada jika diisi
    IF @SalesAgreementID IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SalesAgreement WHERE SalesAgreementID = @SalesAgreementID)
    BEGIN
        RAISERROR('SalesAgreementID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Validasi: pastikan tipe notifikasi tidak kosong
    IF @NotificationType IS NULL OR LEN(@NotificationType) = 0
    BEGIN
        RAISERROR('NotificationType tidak boleh kosong.', 16, 1);
        RETURN;
    END

    -- Validasi: pastikan pesan tidak kosong
    IF @Message IS NULL OR LEN(@Message) = 0
    BEGIN
        RAISERROR('Message tidak boleh kosong.', 16, 1);
        RETURN;
    END

    -- Insert notifikasi baru
    INSERT INTO CustomerNotification (CustomerID, LOIID, SalesAgreementID, NotificationType, Message, CreatedDate)
    VALUES (@CustomerID, @LOIID, @SalesAgreementID, @NotificationType, @Message, GETDATE());

    PRINT 'Notifikasi pelanggan berhasil dibuat.';
END
GO