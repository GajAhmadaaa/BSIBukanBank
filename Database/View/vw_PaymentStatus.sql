CREATE VIEW vw_PaymentStatus AS
SELECT
    sa.SalesAgreementID,
    c.Name AS CustomerName,
    sa.TransactionDate,
    sa.TotalAmount,
    SUM(ph.PaymentAmount) AS TotalPaid,
    CASE
        WHEN sa.TotalAmount IS NOT NULL AND SUM(ph.PaymentAmount) >= sa.TotalAmount THEN 'Paid'
        ELSE 'Unpaid'
    END AS PaymentStatus,
    COUNT(ph.PaymentHistoryID) AS PaymentCount
FROM
    SalesAgreement sa
    JOIN Customer c ON sa.CustomerID = c.CustomerID
    LEFT JOIN PaymentHistory ph ON sa.SalesAgreementID = ph.SalesAgreementID
GROUP BY
    sa.SalesAgreementID, c.Name, sa.TransactionDate, sa.TotalAmount;
GO