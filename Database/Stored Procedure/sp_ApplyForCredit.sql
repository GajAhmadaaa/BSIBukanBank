CREATE PROCEDURE sp_ApplyForCredit
    @LOIID INT,
    @LeasingCompanyID INT,
    @ApplicationDate DATETIME
AS
BEGIN
    INSERT INTO CreditApplication (LOIID, LeasingCompanyID, ApplicationDate, Status)
    VALUES (@LOIID, @LeasingCompanyID, @ApplicationDate, 'Pending');

    SELECT SCOPE_IDENTITY() AS CreditAppID;
END
GO