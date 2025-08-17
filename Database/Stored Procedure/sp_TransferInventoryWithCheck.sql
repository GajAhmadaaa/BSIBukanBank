-- =============================================
-- Author:		<Sales System Development Team>
-- Create date: <Current Date>
-- Description:	Stored Procedure untuk melakukan transfer inventory antar dealer dengan pengecekan ketersediaan stok
-- =============================================
CREATE PROCEDURE [dbo].[sp_TransferInventoryWithCheck]
	@FromDealerID int,
	@ToDealerID int,
	@CarModel varchar(100),
	@Quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Cek ketersediaan stok di dealer asal
	DECLARE @AvailableStock int
	SELECT @AvailableStock = Quantity 
	FROM DealerInventory 
	WHERE DealerID = @FromDealerID AND CarModel = @CarModel
	
	-- Jika stok tidak mencukupi, batalkan transfer
	IF @AvailableStock < @Quantity
	BEGIN
		RAISERROR('Stok tidak mencukupi di dealer asal untuk model mobil yang diminta.', 16, 1)
		RETURN
	END
	
	-- Kurangi stok di dealer asal
	UPDATE DealerInventory 
	SET Quantity = Quantity - @Quantity
	WHERE DealerID = @FromDealerID AND CarModel = @CarModel
	
	-- Tambah stok di dealer tujuan
	IF EXISTS (SELECT 1 FROM DealerInventory WHERE DealerID = @ToDealerID AND CarModel = @CarModel)
	BEGIN
		UPDATE DealerInventory 
		SET Quantity = Quantity + @Quantity
		WHERE DealerID = @ToDealerID AND CarModel = @CarModel
	END
	ELSE
	BEGIN
		INSERT INTO DealerInventory (DealerID, CarModel, Quantity)
		VALUES (@ToDealerID, @CarModel, @Quantity)
	END
	
	-- Catat transfer di tabel InventoryTransfer
	INSERT INTO InventoryTransfer (FromDealerID, ToDealerID, CarModel, Quantity, TransferDate)
	VALUES (@FromDealerID, @ToDealerID, @CarModel, @Quantity, GETDATE())
	
	-- Update status LOI yang terkait dengan dealer tujuan menjadi ReadyForAgreement jika ada
	UPDATE LetterOfIntent 
	SET Status = 'ReadyForAgreement'
	WHERE DealerID = @ToDealerID 
	AND LOIID IN (
		SELECT LOIID 
		FROM LetterOfIntentDetail 
		WHERE CarModel = @CarModel 
		GROUP BY LOIID 
		HAVING COUNT(*) <= @Quantity
	)
	AND Status = 'PendingStock'
	
	SELECT 'Transfer berhasil dilakukan.' AS Message
END