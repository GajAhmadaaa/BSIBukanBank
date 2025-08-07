CREATE TRIGGER trg_UpdateStockAfterSale
ON SalesAgreementDetail
AFTER INSERT
AS
BEGIN
    UPDATE DealerInventory
    SET Stock = Stock - 1
    FROM DealerInventory i
    JOIN inserted ins ON i.CarID = ins.CarID
    JOIN SalesAgreement sa ON ins.SalesAgreementID = sa.SalesAgreementID
    WHERE i.DealerID = sa.DealerID;
END
GO