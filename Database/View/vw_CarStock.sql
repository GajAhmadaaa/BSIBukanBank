CREATE VIEW vw_CarStock AS
SELECT
    d.DealerID,
    d.Name AS DealerName,
    c.CarID,
    c.Model AS CarModel,
    c.CarType,
    c.BasePrice,
    i.Stock AS StockDealer,
    i.Price,
    i.DiscountPercent,
    i.FeePercent
FROM
    DealerInventory i
    JOIN Car c ON i.CarID = c.CarID
    JOIN Dealer d ON i.DealerID = d.DealerID
WHERE
    i.Stock > 0;
GO