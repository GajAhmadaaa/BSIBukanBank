CREATE PROCEDURE sp_CreateVehicleRegistration
    @SalesAgreementDetailID INT,
    @RegistrationNumber VARCHAR(20),
    @OwnershipBookNumber VARCHAR(20),
    @TaxStatus VARCHAR(20),
    @InsuranceStatus VARCHAR(20)
AS
BEGIN
    -- Validasi: pastikan detail transaksi ada
    IF NOT EXISTS (SELECT 1 FROM SalesAgreementDetail WHERE SalesAgreementDetailID = @SalesAgreementDetailID)
    BEGIN
        RAISERROR('SalesAgreementDetailID tidak ditemukan.', 16, 1);
        RETURN;
    END

    -- Validasi: pastikan administrasi untuk unit ini belum ada
    IF EXISTS (SELECT 1 FROM VehicleRegistration WHERE SalesAgreementDetailID = @SalesAgreementDetailID)
    BEGIN
        RAISERROR('Vehicle Registration untuk unit ini sudah ada.', 16, 1);
        RETURN;
    END

    -- Insert administrasi baru
    INSERT INTO VehicleRegistration (SalesAgreementDetailID, RegistrationNumber, OwnershipBookNumber, TaxStatus, InsuranceStatus)
    VALUES (@SalesAgreementDetailID, @RegistrationNumber, @OwnershipBookNumber, @TaxStatus, @InsuranceStatus);

    PRINT 'Vehicle Registration berhasil dibuat.';
END
GO