CREATE VIEW vw_WarrantyClaimStatus AS
SELECT
    wc.WarrantyClaimID,
    c.Name AS CustomerName,
    sa.SalesAgreementID,
    sad.SalesAgreementDetailID,
    sa.TransactionDate,
    wc.ClaimDate,
    wc.Description AS ClaimDescription,
    wc.Status AS ClaimStatus,
    car.Model AS CarModel
FROM
    WarrantyClaim wc
    JOIN Customer c ON wc.CustomerID = c.CustomerID
    JOIN SalesAgreementDetail sad ON wc.SalesAgreementDetailID = sad.SalesAgreementDetailID
    JOIN SalesAgreement sa ON sad.SalesAgreementID = sa.SalesAgreementID
    JOIN Car car ON sad.CarID = car.CarID;
GO