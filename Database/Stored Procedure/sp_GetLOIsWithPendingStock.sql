-- =============================================
-- Author:		<Sales System Development Team>
-- Create date: <Current Date>
-- Description:	Stored Procedure untuk mendapatkan LOI dengan status PendingStock
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLOIsWithPendingStock]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		loi.LOIID,
		c.Name AS CustomerName,
		d.Name AS DealerName,
		loi.LOIDate,
		loi.Status
	FROM LetterOfIntent loi
	INNER JOIN Customer c ON loi.CustomerID = c.CustomerID
	INNER JOIN Dealer d ON loi.DealerID = d.DealerID
	WHERE loi.Status = 'PendingStock'
	ORDER BY loi.LOIDate DESC
END