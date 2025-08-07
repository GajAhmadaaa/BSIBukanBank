CREATE VIEW vw_CreditStatus AS
SELECT
    ca.CreditAppID,
    c.Name AS CustomerName,
    l.Name AS LeasingCompanyName,
    ca.ApplicationDate,
    ca.Status
FROM
    CreditApplication ca
    JOIN LetterOfIntent loi ON ca.LOIID = loi.LOIID
    JOIN Customer c ON loi.CustomerID = c.CustomerID
    JOIN LeasingCompany l ON ca.LeasingCompanyID = l.LeasingCompanyID;
GO