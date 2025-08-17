CREATE PROCEDURE sp_MarkAllCustomerNotificationsAsRead
    @CustomerID INT
AS
BEGIN
    -- Validasi: pastikan customer ada
    IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerID = @CustomerID)
    BEGIN
        RAISERROR('CustomerID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Update semua notifikasi customer menjadi sudah dibaca
    UPDATE CustomerNotification
    SET IsRead = 1, ReadDate = GETDATE()
    WHERE CustomerID = @CustomerID AND IsRead = 0;

    PRINT 'Semua notifikasi pelanggan berhasil diperbarui menjadi sudah dibaca.';
END
GO