CREATE FUNCTION fn_GetDiscountAmount
(
    @LOIDetailID INT
)
RETURNS MONEY
AS
BEGIN
    DECLARE @Discount MONEY
   SELECT @Discount = di.Price * di.DiscountPercent / 100.0
   FROM LetterOfIntentDetail lod
   JOIN LetterOfIntent loi ON lod.LOIID = loi.LOIID
   JOIN DealerInventory di ON loi.DealerID = di.DealerID AND lod.CarID = di.CarID
   WHERE lod.LOIDetailID = @LOIDetailID
    RETURN ISNULL(@Discount, 0)
END
GO