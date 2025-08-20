-- =============================================
-- Author:		<Sales System Development Team>
-- Create date: <Current Date>
-- Description:	Function untuk memverifikasi apakah dealer asal memiliki stok yang cukup untuk dipindahkan
-- =============================================
CREATE FUNCTION [dbo].[fn_CheckTransferFeasibility]
(
	@DealerID int,
	@CarID int,
	@RequestedQuantity int
)
RETURNS bit
AS
BEGIN
	DECLARE @IsFeasible bit = 0
	DECLARE @AvailableStock int
	
	-- Cek stok yang tersedia di dealer
	SELECT @AvailableStock = ISNULL(Stock, 0)
	FROM DealerInventory 
	WHERE DealerID = @DealerID AND CarID = @CarID
	
	-- Jika stok mencukupi, set nilai kelayakan menjadi 1 (true)
	IF @AvailableStock >= @RequestedQuantity
		SET @IsFeasible = 1
	
	RETURN @IsFeasible
END