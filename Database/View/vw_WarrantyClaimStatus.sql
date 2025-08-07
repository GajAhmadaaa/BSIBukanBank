CREATE VIEW vw_WarrantyClaimStatus AS
SELECT
    wc.WarrantyClaimID,
    c.Name AS CustomerName,
    sa.SalesAgreementID,
    sa.TransactionDate,
    wc.ClaimDate,
    wc.Description AS ClaimDescription,
    wc.Status AS ClaimStatus
FROM
    WarrantyClaim wc
    JOIN Customer c ON wc.CustomerID = c.CustomerID
    LEFT JOIN SalesAgreement sa ON wc.SalesAgreementID = sa.SalesAgreementID;
GO