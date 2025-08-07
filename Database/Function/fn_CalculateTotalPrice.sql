CREATE FUNCTION fn_CalculateTotalPrice
(
    @Price MONEY,
    @Discount MONEY
)
RETURNS MONEY
AS
BEGIN
    RETURN @Price - ISNULL(@Discount, 0);
END
GO