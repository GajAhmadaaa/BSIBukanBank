CREATE PROCEDURE sp_MarkCustomerNotificationAsRead
    @CustomerNotificationID INT
AS
BEGIN
    -- Validasi: pastikan notifikasi ada
    IF NOT EXISTS (SELECT 1 FROM CustomerNotification WHERE CustomerNotificationID = @CustomerNotificationID)
    BEGIN
        RAISERROR('CustomerNotificationID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Update status notifikasi menjadi sudah dibaca
    UPDATE CustomerNotification
    SET IsRead = 1, ReadDate = GETDATE()
    WHERE CustomerNotificationID = @CustomerNotificationID;

    PRINT 'Status notifikasi berhasil diperbarui menjadi sudah dibaca.';
END
GO