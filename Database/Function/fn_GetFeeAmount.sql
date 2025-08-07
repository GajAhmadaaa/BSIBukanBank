CREATE FUNCTION fn_GetFeeAmount
(
    @LOIDetailID INT
)
RETURNS MONEY
AS
BEGIN
    DECLARE @Fee MONEY
   SELECT @Fee = di.Price * di.FeePercent / 100.0
   FROM LetterOfIntentDetail lod
   JOIN LetterOfIntent loi ON lod.LOIID = loi.LOIID
   JOIN DealerInventory di ON loi.DealerID = di.DealerID AND lod.CarID = di.CarID
   WHERE lod.LOIDetailID = @LOIDetailID
    RETURN ISNULL(@Fee, 0)
END
GO