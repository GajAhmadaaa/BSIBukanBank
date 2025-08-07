CREATE VIEW vw_SalesReport AS
SELECT
    sa.SalesAgreementID,
    sa.TransactionDate,
    c.Name AS CustomerName,
    sp.Name AS SalesPersonName,
    cr.Model AS CarModel,
    sad.Price,
    sad.Discount,
    dbo.fn_CalculateTotalPrice(sad.Price, sad.Discount) AS TotalPrice
FROM
    SalesAgreement sa
    JOIN SalesAgreementDetail sad ON sa.SalesAgreementID = sad.SalesAgreementID
    JOIN Customer c ON sa.CustomerID = c.CustomerID
    JOIN SalesPerson sp ON sa.SalesPersonID = sp.SalesPersonID
    JOIN Car cr ON sad.CarID = cr.CarID
GO