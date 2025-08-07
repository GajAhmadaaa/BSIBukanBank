CREATE PROCEDURE sp_CreateCarDelivery
    @SalesAgreementID INT
AS
BEGIN
    INSERT INTO CarDelivery (SalesAgreementID, DeliveryDate, Status)
    VALUES (@SalesAgreementID, GETDATE(), 'Delivered');

    UPDATE SalesAgreement SET Status = 'Completed' WHERE SalesAgreementID = @SalesAgreementID;
END
GO