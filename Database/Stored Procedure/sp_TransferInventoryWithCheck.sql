-- =============================================
-- Author:		<Sales System Development Team>
-- Create date: <Current Date>
-- Description:	Stored Procedure untuk melakukan transfer inventory antar dealer dengan pengecekan ketersediaan stok
-- =============================================
CREATE PROCEDURE [dbo].[sp_TransferInventoryWithCheck]
	@FromDealerID int,
	@ToDealerID int,
	@CarID int,
	@Quantity int
AS
BEGIN
	SET NOCOUNT ON;

    -- Cek ketersediaan stok di dealer asal menggunakan fungsi yang sudah diperbaiki
	IF dbo.fn_CheckTransferFeasibility(@FromDealerID, @CarID, @Quantity) = 0
	BEGIN
		RAISERROR('Stok tidak mencukupi di dealer asal untuk model mobil yang diminta.', 16, 1)
		RETURN
	END
	
	-- Kurangi stok di dealer asal
	UPDATE DealerInventory 
	SET Stock = Stock - @Quantity
	WHERE DealerID = @FromDealerID AND CarID = @CarID
	
	-- Tambah stok di dealer tujuan
	IF EXISTS (SELECT 1 FROM DealerInventory WHERE DealerID = @ToDealerID AND CarID = @CarID)
	BEGIN
		UPDATE DealerInventory 
		SET Stock = Stock + @Quantity
		WHERE DealerID = @ToDealerID AND CarID = @CarID
	END
	ELSE
	BEGIN
        -- Jika mobil belum ada di dealer tujuan, buat entri baru.
        -- Ambil harga dasar dari mobil sebagai harga jual awal.
        DECLARE @Price MONEY
        SELECT @Price = BasePrice FROM Car WHERE CarID = @CarID

		INSERT INTO DealerInventory (DealerID, CarID, Stock, Price, DiscountPercent, FeePercent)
		VALUES (@ToDealerID, @CarID, @Quantity, @Price, 0, 0)
	END
	
	-- Catat transfer di tabel InventoryTransfer
	INSERT INTO InventoryTransfer (FromDealerID, ToDealerID, CarID, Quantity, MutationDate)
	VALUES (@FromDealerID, @ToDealerID, @CarID, @Quantity, GETDATE())
	
	

	SELECT 'Transfer berhasil dilakukan.' AS Message
END