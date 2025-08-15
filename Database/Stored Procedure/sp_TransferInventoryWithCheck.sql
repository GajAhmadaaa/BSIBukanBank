CREATE PROCEDURE sp_TransferInventoryWithCheck
    @FromDealerID INT,
    @ToDealerID INT,
    @CarID INT,
    @Quantity INT,
    @MutationDate DATETIME
AS
BEGIN
    -- Mulai transaction
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Cek kelayakan transfer menggunakan function
        DECLARE @IsFeasible BIT;
        SET @IsFeasible = dbo.fn_CheckTransferFeasibility(@FromDealerID, @CarID, @Quantity);
        
        -- Jika tidak layak, batalkan transfer
        IF @IsFeasible = 0
        BEGIN
            RAISERROR('Transfer tidak dapat dilakukan karena stok di dealer asal tidak mencukupi.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Jika layak, lanjutkan dengan transfer
        -- Kurangi stok di dealer asal
        UPDATE DealerInventory
        SET Stock = Stock - @Quantity
        WHERE DealerID = @FromDealerID AND CarID = @CarID;
        
        -- Tambah stok di dealer tujuan
        -- Cek apakah mobil sudah ada di inventori dealer tujuan
        IF EXISTS (SELECT 1 FROM DealerInventory WHERE DealerID = @ToDealerID AND CarID = @CarID)
        BEGIN
            -- Jika sudah ada, update stok
            UPDATE DealerInventory
            SET Stock = Stock + @Quantity
            WHERE DealerID = @ToDealerID AND CarID = @CarID;
        END
        ELSE
        BEGIN
            -- Jika belum ada, tambahkan entri baru
            -- Untuk harga, diskon, dan fee, kita ambil dari dealer asal sebagai default
            DECLARE @Price MONEY, @DiscountPercent FLOAT, @FeePercent FLOAT;
            SELECT @Price = Price, @DiscountPercent = DiscountPercent, @FeePercent = FeePercent
            FROM DealerInventory
            WHERE DealerID = @FromDealerID AND CarID = @CarID;
            
            INSERT INTO DealerInventory (DealerID, CarID, Stock, Price, DiscountPercent, FeePercent)
            VALUES (@ToDealerID, @CarID, @Quantity, ISNULL(@Price, 0), ISNULL(@DiscountPercent, 0), ISNULL(@FeePercent, 0));
        END
        
        -- Catat transfer di tabel InventoryTransfer
        INSERT INTO InventoryTransfer (FromDealerID, ToDealerID, CarID, Quantity, MutationDate)
        VALUES (@FromDealerID, @ToDealerID, @CarID, @Quantity, @MutationDate);
        
        -- Commit transaction
        COMMIT TRANSACTION;
        PRINT 'Transfer stok berhasil dilakukan.';
    END TRY
    BEGIN CATCH
        -- Rollback transaction jika terjadi error
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        -- Re-throw error
        THROW;
    END CATCH
END
GO