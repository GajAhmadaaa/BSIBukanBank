CREATE VIEW vw_AvailableCars AS
SELECT
    d.Name AS DealerName,
    c.Model AS CarModel,
    c.CarType,
    c.BasePrice,
    i.Stock,
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