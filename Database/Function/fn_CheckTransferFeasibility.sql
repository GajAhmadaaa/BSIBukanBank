CREATE FUNCTION fn_CheckTransferFeasibility
(
    @FromDealerID INT,
    @CarID INT,
    @Quantity INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @IsFeasible BIT = 0;
    DECLARE @AvailableStock INT;

    SELECT @AvailableStock = Stock
    FROM DealerInventory
    WHERE DealerID = @FromDealerID AND CarID = @CarID;

    IF ISNULL(@AvailableStock, 0) >= @Quantity
        SET @IsFeasible = 1;

    RETURN @IsFeasible;
END
GO